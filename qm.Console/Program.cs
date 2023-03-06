// See https://aka.ms/new-console-template for more information
using qm.algorithm;
using qm.naive;
using qm.utils;

Console.WriteLine("Hello, World!");


// invoke generator or solution solving


var edges = new byte[4][];

edges[0] = new byte[4] { 0, 1, 0, 1 };
edges[1] = new byte[4] { 0, 0, 1, 1 };
edges[2] = new byte[4] { 1, 0, 0, 1 };
edges[3] = new byte[4] { 0, 0, 0, 0 };

Console.WriteLine("Naive algorithm");

var naiveAlg = new NaiveAlgorithm(4, edges);
Console.WriteLine(Helpers.FormatResult(naiveAlg.ConductAlgorithm()));

Console.WriteLine("Qm algorithm");

var qmAlg = new QmAlgorithm(4, edges);
Console.WriteLine(Helpers.FormatResult(qmAlg.ConductAlgorithm()));