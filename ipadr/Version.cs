﻿
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
namespace CommandLine
{
    public class Version
    {
        public int major { get; set; }
        public int minor { get; set; }
        public int build { get; set; }
        public string name { get; set; }
        public Version(int m, int n, int b, string x)
        {
            major = m;
            minor = n;
            build = b;
            name = x;
        }
        public override string ToString()
        {
            return name
                 + major.ToString() + "."
                 + minor.ToString() + "."
                 + build.ToString();
        }
    }
}
