# NMEAParserNET

## Abstract
This library uses the NMEA schema library.
It is intended for use as an intermediate layer for various applications that use NMEA.

## Target

.NET 5.0

## Installation
### Using dotnet cli
```
dotnet add package nmeaparsernet
```

## Usage

### Convert GGA
```cs
var gga = new GGA("GNGGA,035817.30,3444.3418019,N,13535.1207337,E,5,12,2.59,25.572,M,34.179,M,1.3,0000*50");
```

### Determine NEMA message type
```cs
var type = NMEAParserNET.Parser.JudgeMsgType(sentence);
```

### Remove non-sentence binaries
if you used u-blox module. 
Binaries and sentences are mixed in the output file.

```cs
string str = NMEAParserNET.Parser.PurgeWithinNMEA(filepath);
```


## Attention!!
This library is under development and has not been fully tested.

We are looking for a collaborator.
