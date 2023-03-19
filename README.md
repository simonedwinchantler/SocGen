# SocGen
Market Data Project

There are three peojects in this solution, MarketDataExchange is the server process, mdx is a test client project and mdxValidation is the stub validation project.

The main idea is to be able to persists XML docs of whatever flavour in locations specified by the user - rather like a directory structure in a filesystem.

The real inplementation would expect to place limits on the sections that make up the overall identifier:

"/fxrates/london/eod/GBPUSD@20230418"

indicates fx type data, then some sense of who this data is owned by or expected to be used by, then some sense of what it is an EOD snap for exmaple, then the final elements are the name in this case GBPUSD and the business date which this refers to.

We only ever insert data never update it. 

The implementation returns the 'latest' item for a given identifier that can be extended to return a specific version.

The test client runs through the various end points to first get a 'connection' to the server, then contributes two XML versions of the GBPUSD XML doc, then reads the latest back in.



