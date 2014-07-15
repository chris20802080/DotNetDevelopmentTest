In app.config there is a setting for the data file directory.
Place all test data files in this directory. They should be named <testname>.config. (ex: 3.config)



Command Line Format



Short Version

Airstrip.Simulator <testname>

Example: 
Airstrip.Simulator 3



Long Version

Airstrip.Simulator -scenario <scenario name> (-verbose) ... other switches specific to the scenario

For the GroceryStore scenario, the extra required switches are:

-testsource [db|file] -testname <testname>

Example: Airstrip.Simulator -verbose -scenario GroceryStore -testsource file -testname 3