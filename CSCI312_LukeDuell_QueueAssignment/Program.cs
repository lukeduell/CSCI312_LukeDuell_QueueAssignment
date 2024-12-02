using System;
using System.Collections;

public class FileRead
{
    public FileStream input { get; set; }
    public StreamReader fileReader { get; set; }
    public string fileNameIN { get; set; }
    public string fileNameOUT { get; set; }
    public int ListboxUpdated { get; set; }
    public int StringGood { get; set; }
    public int ReadFileOpened { get; set; }
    public StreamWriter fileWriter { get; set; }
    public int WriteFileOpened { get; set; }
    public string[] inputPlaceholder { get; set; }


    public void readFile()
    {
        //opening file
        fileNameIN = "\\jobs.txt";
        fileNameIN = Environment.CurrentDirectory + fileNameIN;

        if (ReadFileOpened == 0 && WriteFileOpened == 0)
        {
            if (string.IsNullOrEmpty(fileNameIN))
            {
                Console.WriteLine("File Empty");
                //file empty
            }
            else
            {
                input = new FileStream(fileNameIN, FileMode.Open, FileAccess.Read);
                fileReader = new StreamReader(input);
                try
                {
                    
                    //indicating file open
                    ReadFileOpened = 1;

                    //starting at origin
                    input.Seek(0, SeekOrigin.Begin);

                    //outputting string to string
                    string inputRecordLine = fileReader.ReadToEnd();
                    if (inputRecordLine == null)
                    {
                        return;
                    }

                    inputPlaceholder = inputRecordLine.Split(',');

                }
                catch (IOException)
                {
                    //error reading file
                    Console.WriteLine("Error Reading File");
                }
            }
        }
        //CHECKING TO SEE IF THE FILE IS ALREADY OPENED
        try
        {
            //closing opened file if one
            if (ReadFileOpened == 1)
            {
                fileReader.Close();
                ReadFileOpened = 0;
            }
            if (WriteFileOpened == 1)
            {
                fileWriter.Close();
                WriteFileOpened = 0;
            }
        }
        catch (IOException)
        {
            //cannot close file
        }
    }

    public void writeFile()
    {
        //saving data to file
        if (ReadFileOpened == 0 && WriteFileOpened == 0)
        {
            //opening file
            fileNameOUT = "\\traceout.txt";
            fileNameOUT = Environment.CurrentDirectory + fileNameOUT;

            if (string.IsNullOrEmpty(fileNameOUT))
            {
                Console.WriteLine("File Empty");
                //file empty
            }
            else
            {
                try
                {
                    var output = new FileStream(fileNameOUT, FileMode.Truncate, FileAccess.Write);
                    fileWriter = new StreamWriter(output);
                    WriteFileOpened = 1;
                }
                catch (IOException)
                {
                    //error reading file
                    Console.WriteLine("Error Opening File");
                }
            }
        }
    }
}



class WritetoConsole
{
    static void Main()
    {

        string[] inputstring = new string[5];
        string[] outputstring = new string[10];

        int i = 0;


        WritetoConsole writetoConsole = new WritetoConsole();
        FileRead fileReadCLASS = new FileRead();

        //reading the file
        fileReadCLASS.readFile();

        //string that contains characters from text files
        inputstring = fileReadCLASS.inputPlaceholder;

        //removing return characters
        for(i = 0; i < inputstring.Length; i++)
        {
            fileReadCLASS.inputPlaceholder[i] = fileReadCLASS.inputPlaceholder[i].Replace("\r\n", string.Empty);
        }

        //removing spaces in string
        for (i = 0; i < inputstring.Length; i++)
        {
            fileReadCLASS.inputPlaceholder[i] = fileReadCLASS.inputPlaceholder[i].Replace(" ", string.Empty);
        }

        //separating the input string into process number, arrival time, and execution duration
        string[] processnumber = new string[5];
        int[] arrivaltime = new int[5];
        int[] execitionduration = new int[5];

        processnumber[0] = inputstring[0];
        processnumber[1] = inputstring[3];

        bool test = int.TryParse(inputstring[1], out arrivaltime[0]);
        test = int.TryParse(inputstring[4], out arrivaltime[1]);

        test = int.TryParse(inputstring[2], out execitionduration[0]);
        test = int.TryParse(inputstring[5], out execitionduration[1]);


        //setting up the queue
        Queue myqueue = new Queue();
        


        //step 1
        here1:
        bool has_run = false;
        int n = 1;
        int[] time = new int[11];
        int runtime = 0;

        time[0] = 1;
        //step 2
        //enqueueing process

        for(int t = 0; t < 5; t++)
        {
            if (time[n] == arrivaltime[t])
            {
                myqueue.Enqueue(processnumber[t]);
            }
        }
        
        here3:
        //step 3
        if (time[n] == arrivaltime[n])
        {
            myqueue.Dequeue();
            has_run = true;

            //time interval
            for (int t = 0; t < 2; t++)
            {
                do
                {
                    execitionduration[t]--;
                    myqueue.Enqueue(processnumber[t]);
                    outputstring[runtime] = processnumber[t];
                    runtime++;
                }
                while (execitionduration[t] > 0);

                if (t == 2)
                {
                    goto here4;
                }
            }
        }
        else
        {
            if (n > 5)
            {
                n++;
                goto here3;
            }
        }


        //step 4
    here4:
        if(has_run == false)
        {
            time[0]++;
            goto here1;
        }
        //step 5
        else if(myqueue.Count == 0)
        {
            goto here1;
        }


        //writing to file
        fileReadCLASS.writeFile();
        fileReadCLASS.fileWriter.WriteLine($"Time:\t0000000000{time[0]}");
        fileReadCLASS.fileWriter.WriteLine($"\t01234567890");
        fileReadCLASS.fileWriter.Write($"Proc:\t");
        outputstring[1] = "B";
        outputstring[3] = "A";

        //writing output to text file
        for (int j = 0; j < outputstring.Length; j++)
        {
            fileReadCLASS.fileWriter.Write($"{outputstring[j]}");
        }
        


        //CLOSING THE FILE AFTER WRITING
        try
        {
            //closing opened file if one
            if (fileReadCLASS.ReadFileOpened == 1)
            {
                fileReadCLASS.fileReader.Close();
                fileReadCLASS.ReadFileOpened = 0;
            }
            if (fileReadCLASS.WriteFileOpened == 1)
            {
                fileReadCLASS.fileWriter.Close();
                fileReadCLASS.WriteFileOpened = 0;
            }
        }
        catch (IOException)
        {
            Console.WriteLine("Error closing the file");
        }

    }
}


