﻿using System;
using System.Collections.Generic;

namespace ppm
{
    public class Membership
    {
        private string context, username, password;
        private Membership()
        {

        }
        public Membership(string ctx, string uname, string pwd)
        {
            context = ctx;
            username = uname;
            password = pwd;
        }
        public string Update(string db)
        {
            return db + context + ":" + username + ":" + password + ":\n";
        }
        public void Parse(string line)
        {
            string[] fields = line.Split(':');
            context = fields[0];
            username = fields[1];
            password = fields[2];
        }
        public void Show()
        {
            Console.WriteLine($"Context={context} Username={username} Password={password}");
        }
        public bool Matches(string ctx, string uname)
        {

            if (ctx.Equals(context) && uname.Equals(username))
            {
                return true;
            }
            return false;
        }
        static public string All(Membership []members)
        {
            string db = "";
            foreach (Membership m in members)
            {
                db = m.Update(db);
            }
            return db;
        }

        static public void ShowAll(Membership[] members)
        {
            foreach (Membership m in members)
            {
                m.Show();
            }
        }
        static public string Add(string db, Membership m)
        {
            return m.Update(db);
        }
        public void SetPassword(string newp)
        {
            password = newp;
        }
        static public Membership[] ParseDb(string db)
        {
            List<Membership> mlist = new List<Membership>();
            string[] lines = db.Split('\n');
            foreach (string line in lines)
            {
                if (!line.StartsWith("#"))
                {
                    if (line.Length > 1)
                    {
                        Membership m = new Membership();
                        // Console.WriteLine($"Parsing {line}");
                        m.Parse(line);
                        mlist.Add(m);
                    }
                }
            }
            return mlist.ToArray();
        }
    }
}
