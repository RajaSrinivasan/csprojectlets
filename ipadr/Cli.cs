// Copyright © 2019 TOPR llc.

// Permission to use, copy, modify, and/or distribute this software for any purpose with or without fee is 
// hereby granted, provided that the above copyright notice and this permission notice appear in all copies.

// THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH REGARD TO THIS SOFTWARE
// INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS.IN NO EVENT SHALL THE AUTHOR BE LIABLE
// FOR ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES WHATSOEVER RESULTING FROM
// LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION,
// ARISING OUT OF OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.

// All questions may please be addressed to contact@toprllc.com

using System;
using System.Collections.Generic;
namespace CommandLine
{
    public class Cli
    {
        public struct Subcommand
        {
            public string subcmd;
            public string desc;
            public Subcommand(string sc, string d)
            {
                subcmd = sc;
                desc = d;
            }
        }

        public List<Subcommand> subcommands = new List<Subcommand>();

        public struct Switch
        {
            public string shortform;
            public string longform;
            public string description;
            public string argopts;
            public string def;

            /*
             *
             
   --  Options
   --
   --   ':'  The switch requires a parameter. 
   --
   --   '?'  The switch may have an optional parameter. if the next arg is not a switch, it is the
   --        parameter for this switch

             */

            public Switch(string sf, string lf, string desc, string opt, string d)
            {
                shortform =  sf;
                longform = lf;
                description = desc;
                if (opt == null)
                {
                    argopts = null;
                }
                else
                { 
                    if (opt.Equals("?") || opt.Equals(":"))
                        argopts = opt;
                    else
                    {
                        Console.WriteLine("Unrecognized option : {0,0} for switch {1,0}", opt , sf);
                        argopts = null;
                    }
                }
                def = d;
            }
        }


        public List<Switch> switches = new List<Switch>();
        public Version version { get; set; }
        public string subcommand {get; set;}

        private Dictionary<string, string> used_switches = new Dictionary<string, string>();

        private int lastswitch = -1;
        private List<string> operands = new List<string>();

        public string usage_text { get; set; }

        public Cli()
        {
            AddSwitch("h", "help", "showhelp", null , "false" );
            AddSwitch("v", "verbose", "be verbose", null , "true" );
        }

        public void Help()
        {
            if (version != null)
            {
                Console.WriteLine($"{version.name} - Version {version.major}.{version.minor}-{version.build}");
            }
            if (usage_text != null)
            {
                Console.WriteLine(usage_text);
            }
            if (subcommands.Count > 1)
            {
                Console.WriteLine($"Subcommands: ");
                foreach (Subcommand sc in subcommands)
                {
                    Console.Write("{0,-12} : " , sc.subcmd);
                    Console.WriteLine(sc.desc);
                }
            }
            Console.WriteLine("Switches");
            Console.Write("{0,-9}", "Short");
            Console.Write("{0,-18}", "Long");
            Console.Write("{0,-8}", "Options");
            Console.Write("{0,-16}", "Default");
            Console.WriteLine("Description");
            foreach (Switch sw in switches)
            {
                Console.Write("{0,-8}", sw.shortform);
                Console.Write("{0,-16}", sw.longform);
                Console.Write("{0,-8}", sw.argopts);
                Console.Write("{0,-16}", sw.def);
                Console.WriteLine(sw.description);
            }
        }

        // ProcessSwitch
        //     returns
        //           0 - if the switch is valid and no change to idx
        //           1 - the switch is valid and required a parameter. The next arg consumed as a paarmeter
        //          -1 - the switch is not recognized
        //          -2 - valid switch but missing required parameter
        private int ProcessSwitch(string[] args, int idx)
        {
            //Console.WriteLine("Processing switch {0,1}", args[idx]);
            string thissw = args[idx];
            if (args[idx].Substring(0, 2).Equals("--"))
            {
                thissw = args[idx].Substring(2);
                //Console.WriteLine("Long Form {0,1}", thissw);
            }
            else if (args[idx].Substring(0, 1).Equals("-"))
            {
                thissw = args[idx].Substring(1);
                //Console.WriteLine("Short Form {0,1}", thissw);
            }
            foreach (Switch sw in switches)
            {
                if (sw.shortform.Equals(thissw) || sw.longform.Equals(thissw))
                {
                    if (sw.argopts == null)
                    {
                        used_switches[thissw] = sw.def;
                        return 0;
                    }
                    else
                    {
                        switch (sw.argopts)
                        {
                            case ":":
                                if (idx > args.Length - 1)
                                {
                                    Console.WriteLine("Switch {0,0} requires a paramater", sw.shortform);
                                    return -2 ;
                                }
                                if (args[idx + 1][0].Equals("-"))
                                {
                                    Console.WriteLine("Switch {0,0} requires a paramater", sw.shortform);
                                    return -2 ;
                                }
                                used_switches[thissw] = args[idx + 1];
                                idx++;
                                return 1;
    
                            case "?":
                                if (idx >= args.Length - 1)
                                {
                                    used_switches[thissw] = sw.def;
                                    //Console.WriteLine("Using default {0,1}", sw.def);
                                    return 0 ;
                                }

                                if (args[idx + 1].Substring(0,1).Equals("-"))
                                {
                                    used_switches[thissw] = sw.def;
                                    //Console.WriteLine("Next arg is a switch. Using default {0,1}", sw.def);
                                    return 0 ;
                                }
                                used_switches[thissw] = args[idx + 1];
                                //Console.WriteLine("Setting value {0,1}", args[idx + 1]);
                                idx++;
                                return 1 ;
                            default:
                                Console.WriteLine("Internal Error #1");
                                break;       
                        }
                    }
                }
            }
            Console.WriteLine("Unrecognized switch {0,0}", args[idx]);
            return -1 ;
        }
        // Analyze - analyze the command line arguments
        //           returns false - to exit right away.
        //                           unrecognized subcommand or switch
        //                           no args
        //                           help requested
        public bool Analyze(string[] args)
        {
            if (args.Length == 0)
            {
                Help();
                return false;
            }

            int nextarg = 0;
            if (subcommands.Count > 0)
            {
                Subcommand []scs = subcommands.ToArray();
                // bool foundsubcmd = false;
                foreach (Subcommand sc in scs)
                {
                    if (sc.subcmd.Equals( args[0]))
                    {
                        subcommand = args[0];
                        nextarg++;
                        // foundsubcmd = true;
                        break;
                    }
                }
            }
            
            //Console.WriteLine("Found a subcommand {0,1}", subcommand);
            int cont = 0;
            while ((cont >= 0) && nextarg < args.Length )
            {
                string issw = args[nextarg];
                //Console.WriteLine("Trying {0,1}", issw);
                if (issw.Substring(0,1).Equals("-"))
                {
                    // found a switch argument.
                    //Console.WriteLine("Found a switch");
                    lastswitch = nextarg;
                    cont = ProcessSwitch(args, nextarg);
                    if (cont == 1)
                    {
                        nextarg++;
                    }

                    if (cont < 0) return false;
                    nextarg++;
                }
                else
                {
                    //Console.WriteLine("Arg is not a switch {0,1}", args[nextarg]);
                    for (int oarg=nextarg; oarg < args.Length; oarg++)
                    {
                        operands.Add(args[oarg]);
                    }
                    break;
                }
                if (nextarg >= args.Length) break;
            }

            return true;
        }
        public void AddSubcommand( string sc, string desc)
        {
            subcommands.Add(new Subcommand( sc, desc ));
        }
        public void AddSwitch( string sf, string lf, string desc, string opt, string d)
        {
            switches.Add(new Switch(sf, lf, desc, opt, d));
        }

        public string GetCommand()
        {
            return subcommand;
        }

        public bool Present(string sw)
        {
            string temp;
            if (used_switches.TryGetValue(sw,out temp))
            {
                return true;
            }
            //Console.WriteLine("Switch {0,1} was not present int the commandline",sw);
            return false;
        }

        public bool GetBool(string sw)
        {
            string[] valid_true = { "T", "true" } ;
            string value;
            if (used_switches.TryGetValue(sw, out value))
            {
                if (Array.IndexOf(valid_true, value) >= 0) return true;
            }
            return false;
        }

        public int GetInt(string sw)
        {
            string value;
            if (used_switches.TryGetValue(sw, out value))
            {
                try
                {
                    return int.Parse(value);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception {0,0}", e.ToString());
                    Console.WriteLine($"Switch value is not an integer. ({value})");
                }
            }
            return 0;
        }

        public string GetString(string sw)
        {
            string value;

            if (used_switches.TryGetValue(sw, out value))
            {
                return value;
            }
            return null;
        }

        public string[] Operands()
        {
            return operands.ToArray();
        }
        public string Operand()
        {
            return string.Join(" ",operands);
        }

    }
}
