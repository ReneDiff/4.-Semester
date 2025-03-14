import dayjs from 'dayjs';
import { useState } from 'react';

type Event = 
{
    id: number;
    title: string;
    time: string;
    location: string;
};

const sampleEvent: Event[] = 
[
    {id: 1, title: "Loppemarked", time: "10:00", location: "Torvet"},
    
];