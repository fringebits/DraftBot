

Handshake status 200 when expecting 101.
Note this information:  https://github.com/mrjoes/tornadio/issues/21
I beleive I need to be connecting to a different endpoint, not the address directly.
ws://10.0.0.213 v. ws://10.0.0.213/


Python library, looks like it connects directly to a socket (port 8899)
https://github.com/mirobot/mirobot-py
https://msdn.microsoft.com/en-us/library/bew39x2a(v=vs.110).aspx (C# direct connect socket example)

