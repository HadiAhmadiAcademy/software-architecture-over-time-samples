using WorldCupStandings;

// 2018 World Cup, Group B
string[] matches =
[
    "MAR 0-1 IRN",
    "POR 3-3 ESP",
    "POR 1-0 MAR",
    "IRN 0-1 ESP",
    "IRN 1-1 POR",
    "ESP 2-2 MAR",
];

Console.Write(GroupStandings.Calculate(matches));
