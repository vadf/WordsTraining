﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsTraining
{
    public abstract class DataLayer
    {
        abstract public WordsDictionary Read();
        abstract public void Save(WordsDictionary dictionary);
    }
}
