﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("CodeQuality", "Serilog004:Constant MessageTemplate verifier", Justification = "<Pending>", Scope = "member", Target = "~M:NeuralJourney.Infrastructure.Engines.ClientEngine.HandleNetworkInputReceived(System.String,System.Net.Sockets.TcpClient)")]
[assembly: SuppressMessage("CodeQuality", "Serilog004:Constant MessageTemplate verifier", Justification = "Function is only used for templates without properties", Scope = "member", Target = "~M:NeuralJourney.Infrastructure.Engines.ClientEngine.LogInfoAndDisplayInConsole(System.String)")]
[assembly: SuppressMessage("CodeQuality", "Serilog004:Constant MessageTemplate verifier", Justification = "Statement is only used for templates without properties", Scope = "member", Target = "~M:NeuralJourney.Infrastructure.Engines.ClientEngine.Run(System.Threading.CancellationToken)~System.Threading.Tasks.Task")]
