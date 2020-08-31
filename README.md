# post-office-problem

# Prerequisites
The framework used in this solution was the .NET Core v3.1.

# Building
1. Clone this repository;
2. Enter the repo folder;
3. Execute the `build.bat` file;

```
> build.bat
```

# Usage
It's possible to execute the program with the following command format:

```
> run.bat graph_edges_file_path jobs_file_path routes_file_path
```

The program accepts at most 3 arguments:

1. **_graph_edges_file_path_** (**Mandatory**): The input file path that contains the graph edges.
2. **_jobs_file_path_** (**Mandatory**): The input file path that contains the jobs to be done.
3. **_routes_file_path_** (**Optional**): The output file path that will contain the shortest paths.
<br>If not informed, it will be created in the directory that the program is called.

So, for instance, you could run with this arguments:
```
> run.bat c:\trechos.txt c:\encomendas.txt c:\rotas.txt
```

# Unit Tests
You can run the unit tests with the following command:
```
> run_tests.bat
```