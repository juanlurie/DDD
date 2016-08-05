﻿namespace Iris.Scheduling.Fields
{
    internal interface ICronField
    {
        bool Contains(int value);
        CronValue GetFirst();
        CronValue GetNext(int currentValue);
        CronValue GetNext(CronValue current);
    }
}