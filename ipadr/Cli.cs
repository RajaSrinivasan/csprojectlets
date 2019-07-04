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
                shortform = sf;
                longform = lf;
                description = desc;
                if (opt == null)
                {
                    argopts = null;
                }
                else
                { 
                    if (opt.Equals("=") || opt.Equals(":"))
                        argopts = opt;
                    else
                    {
                        Console.WriteLine("Unrecognized option : {0,0} for switch {0,1}", opt , sf);
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
                Console.Write("-{0,-8}", sw.shortform);
                Console.Write("--{0,-16}", sw.longform);
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
            foreach (Switch sw in switches)
            {
                if (sw.shortform.Equals(args[idx]) || sw.longform.Equals(args[idx]))
                {
                    if (sw.argopts == null)
                    {
                        used_switches[args[idx]] = sw.def;
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
                                used_switches[args[idx]] = args[idx + 1];
                                idx++;
                                return 1;
    
                            case "=":
                                if (idx > args.Length - 1)
                                {
                                    used_switches[args[idx]] = sw.def;

                                    return 0 ;
                                }
                                if (args[idx + 1][0].Equals("-"))
                                {
                                    used_switches[args[idx]] = sw.def;
                                    return 0 ;
                                }
                                used_switches[args[idx]] = args[idx + 1];
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
                bool foundsubcmd = false;
                foreach (Subcommand sc in scs)
                {
                    if (sc.subcmd.Equals( args[0]))
                    {
                        subcommand = args[0];
                        nextarg++;
                        foundsubcmd = true;
                        break;
                    }
                }
                if (!foundsubcmd)
                {
                    Console.WriteLine("Did not find a valid subcommand.");
                    return false;
                }

            }
            int cont = 0;
            while ((cont >= 0) && nextarg < args.Length )
            {
                if (args[nextarg][0].Equals("-"))
                {
                    // found a switch argument.
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
