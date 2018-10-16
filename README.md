# PDSimulation

How to run this code
Clone or download the repository
Open the solution in visual studio
Build solution
Run solution
Change file paths to here the data is stored, and where you want the data output to go
Change sim number to the number of simulations you need to run
Click go

Data format needed

Subsystem data file 

   C1    
H1 name
R1 name1
R2 name2
R3 name3
   ...
 
 Main data file
   
     C1         C2        C3                          C4                          C5                          C6      C7
 H1  subsystem  tasktime  subsystemdependency [name1] subsystemdependency [name2] subsystemdependency [name3] rework  assumptions  
 R1  name1
 R2  name2
 R3  name3
     ...
     
      C8                 C9               C10             C11         C12                       C13
      assumptionaccuracy assumptioneffect centralization  messagerate totalmessageresponsetime  experience

Data output is as a list of daystaken
