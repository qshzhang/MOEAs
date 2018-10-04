
This project includes seven folders, namely Algorithms, Common, Encoding, POF, Problems, QualityIndicator.
 
           |-- Algorithms: some MOEA ailgorithms are implemented
           |
           |-- Common: common functions
           |
           |-- Encoding: class of chromosome, and some methods are provided, include crossover and mutation operator
           |
MOEAPlat-- |-- POF: include some POFs, and also provide methods to generate POF of some problems
           |
           |-- Problems: provide some MOPs implementation
           |
           |-- QualityIndicator: IGD and HV are provided to quality obtained solutions
           |
           |-- PlotDialog: show the obtained result

If you want to implement a MOEA algorithm, you only need add a .cs file in Algorithms folder, and only the method EnviromentSelection is required to modify. Also, if you want add a MOP, only a .cs file need be generated in Problems folder.  
