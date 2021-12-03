# Decsription

This solution created for reproduce bug, which I found after migration to .net6 with ef core 6 and Pomelo.EntityFrameworkCore.MySql 6.0.0
It contains two projects with same files EfCoreTests.cs and EfSample.cs and different versions of dependencies.

Net5Tests works fine, but Net6Tests failed.

# How to run?

Execute setupDb.sh for prepare mysql database in docker and run test for projects.
