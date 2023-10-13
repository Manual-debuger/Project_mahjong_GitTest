using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.Scripts
{
    public class Parser
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public static List<Tuple<string, string>> Parsing(string target)
        {
            Regex regex = new Regex(@"([^\n]+)[：:]([^\n]+)");
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();
            MatchCollection matches = regex.Matches(target);
            foreach (Match match in matches)
            {
                result.Add(new Tuple<string, string>(match.Groups[1].Value, match.Groups[2].Value));
            }
            return result;
        }
    }
}