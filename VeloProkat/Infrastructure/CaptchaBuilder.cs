﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Капча
namespace VeloProkat.Infrastructure
{
    public class CaptchaBuilder
    {
        public static string Refresh()
        {
        string captcha = "";
        Random rand = new Random();

            for (int i = 0; i < 3; i++)
            {
                captcha += (char)rand.Next('A', 'Z' + 1) + rand.NextInt64(1, 100).ToString();
            }
            return captcha;
        }
    }
}
