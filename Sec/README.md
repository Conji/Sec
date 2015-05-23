Syhno's Easy Config (SEC)
=========================
Based on TOML, just with a few other features that allow for a more seamless integration with Syhno. Not
only for use with Syhno, just the base as what it was meant for.

Declaring a new SecFile:
```
var sec = new SecFile("config"); // this will only set the location of the file
```
To see if the file has any tables that the library can read, you can use this:
```
sec.HasTables
```
If it does, you can now open the file for reading.
```
sec.ReadTables();
```
If you'd like to skip all the hasle and create it at runtime, you can simply use
```
var sec = SecFile.Open("config");
```
This will go ahead and read the config and then pass the tables to data.

Each table in the SecFile has it's own data. This data can be inheritable, depending on how you name it. A
basic table would look like:
```
(server)
address = 192.168.0.7
port = 8080
```
Pretty easy, yeah? Well, to grab the data from this table, it's quite simple.
```
SecFile sec = SecFile.Open("config");
string address = sec["server"].Get<string>("address");
int port = sec["server"].Get<int>("port");
```

Table Inheritance
-----------------
I mentioned that tables can inherit. This is achieved by separating with a '.'. IE:
```
(server)
address = 192.168.0.7
port = 8080

(server.debug)
port = 8181
```

All of the values from 'server' will copy to 'server.debug', but only the last defined values of a
pre-defined key will be used. That means that even though we declared "port" to be 8080, 'server.debug'
will use 8181. This will not affect 'server' though or any other inherited tables unless they also inherit
from 'server.debug'.

To explicitly state inheritance, put a `> %tablename%` after the table name. IE:
```
(server.admin) > server.mod
```
You can have multiple, just place a comma in between.

Sec Objects
-----------
Objects that can be used in the config are:
- string 
- int
- float
- bool

The way the reader will pass objects is simple. Any objects that are not int, float, bool, or array, will
be passed as a string so that no value errors are thrown. 

Table Values
------------
This is completely different from key-value pairs that the table owns. These are keyless values that belong
to the table.
```
(commands)
cd
mkdir
mkfile
rmdir
rmfile
```
These can be accessed via `sec["commands"]`. The only values these can be are strings.
