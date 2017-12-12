using System;
using System.Windows.Forms;
using System.IO;
using DHI.Generic.MikeZero.DFS.dfsu;
using DHI.Generic.MikeZero;

namespace Compute_Direction_Max
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close(); //Close the window when pressing the Close button
        }

        private void btBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "FM result files (*.dfsu)|*.dfsu"; //Define the supported input file type
            openFileDialog1.ShowDialog(); //Open the browse dialog
            txtPath.Text = openFileDialog1.FileName; //Save the path to the selected file in the text field
            if (File.Exists(txtPath.Text)) btStart.Enabled = true; //Enable the Start button if the selected file exists
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioH.Checked == true)
                {
                    ComputeHmax(txtPath.Text); //Call the ComputeHmax method if the selected radio button is 'maximum depth'
                }
                else
                {
                    ComputeVmax(txtPath.Text); //Call the ComputeVmax method if the selected radio button is 'maximum speed'
                }
            }
            catch (FileNotFoundException FEX)
            {
                MessageBox.Show(FEX.Message, FEX.GetType().Name); //Report a 'File Not Found' error, if any
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message, EX.GetType().Name); //Report other errors, if any
            }
        }

        private void ComputeHmax(string filename)
        {
            btStart.Enabled = false; //Disable the Start button, to prevent user from running the tool twice at the same time
            btClose.Enabled = false; //Disable the Close button, to prevent user from closing the tool while running
            //Read input file and find relevant items
            IDfsuFile InputFile = DfsuFile.Open(filename); //Open the file
            int ItemNb = InputFile.ItemInfo.Count; //Number of items in the file
            int ItemNbH = -1; //Stores the item number for water depth. Initialised with a temporary value.
            int ItemNbU = -1; //Stores the item number for U velocity. Initialised with a temporary value.
            int ItemNbV = -1; //Stores the item number for V velocity. Initialised with a temporary value.
            for (int i = 0; i < ItemNb; i++) //Loop finding appropriate items H, U and V
            {
                if (InputFile.ItemInfo[i].Name == "Total water depth") ItemNbH = i; //Save the actual item number when Total water depth is found
                if (InputFile.ItemInfo[i].Name == "U velocity") ItemNbU = i; //Save the actual item number when U velocity is found
                if (InputFile.ItemInfo[i].Name == "V velocity") ItemNbV = i; //Save the actual item number when V velocity is found
            }
            if (ItemNbH == -1 || ItemNbU == -1 || ItemNbV == -1) //If one of the required item cannot be found
            {
                btClose.Enabled = true; //Enable the Close button again
                throw new Exception("The result file doesn't contain the necessary items H, U and V"); //Throw error message
            }
            else
            {
                //Create output file, with same nodes, elements, projection and time info as the input file, but with different output items
                DfsuBuilder OutputBuilder = DfsuBuilder.Create(DfsuFileType.Dfsu2D);
                OutputBuilder.SetNodes(InputFile.X, InputFile.Y, InputFile.Z, InputFile.Code);
                OutputBuilder.SetElements(InputFile.ElementTable);
                OutputBuilder.SetProjection(InputFile.Projection);
                OutputBuilder.SetTimeInfo(InputFile.StartDateTime, InputFile.TimeStepInSeconds);
                OutputBuilder.SetZUnit(InputFile.ZUnit);
                OutputBuilder.AddDynamicItem("Maximum water depth", InputFile.ItemInfo[ItemNbH].Quantity); //Create item H
                OutputBuilder.AddDynamicItem("U velocity @ max. depth", InputFile.ItemInfo[ItemNbU].Quantity); //Create item U
                OutputBuilder.AddDynamicItem("V velocity @ max. depth", InputFile.ItemInfo[ItemNbV].Quantity); //Create item V
                OutputBuilder.AddDynamicItem("Current speed @ max. depth", InputFile.ItemInfo[ItemNbU].Quantity); //Create item Speed
                OutputBuilder.AddDynamicItem("Current direction @ max. depth", eumQuantity.Create(eumItem.eumICurrentDirection, eumUnit.eumUradian)); //Create item Direction. Note: eumQuantity requires "using DHI.Generic.MikeZero"
                //Initialization of all output variables. Both source data and output data are intialized with data from first time step of the input file.
                float[] SourceDataH = (float[])InputFile.ReadItemTimeStep(ItemNbH + 1, 0).Data; //ReadItemTimeStep is 1-based! That is, the first time step must be numbered 1, whereas the first time step in the file is numbered 0, hence the +1.
                float[] SourceDataU = (float[])InputFile.ReadItemTimeStep(ItemNbU + 1, 0).Data;
                float[] SourceDataV = (float[])InputFile.ReadItemTimeStep(ItemNbV + 1, 0).Data;
                float[] OutputDataH = (float[])InputFile.ReadItemTimeStep(ItemNbH + 1, 0).Data;
                float[] OutputDataU = (float[])InputFile.ReadItemTimeStep(ItemNbU + 1, 0).Data;
                float[] OutputDataV = (float[])InputFile.ReadItemTimeStep(ItemNbV + 1, 0).Data;
                float[] OutputDataSpeed = (float[])InputFile.ReadItemTimeStep(ItemNbU + 1, 0).Data; //Initialise speed with values of U at time step 0
                float[] OutputDataDir = (float[])InputFile.ReadItemTimeStep(ItemNbU + 1, 0).Data; //Initialise direction with values of U at time step 0
                for (int m = 0; m < InputFile.NumberOfElements; m++) //Change speed and direction at first time step based on U and V values, with a loop over each element
                {
                    OutputDataSpeed[m] = (float)Math.Sqrt(Math.Pow(SourceDataU[m], 2) + Math.Pow(SourceDataV[m], 2));
                    OutputDataDir[m] = (float)Math.Atan2(SourceDataU[m], SourceDataV[m]);
                }
                //Define the properties of the progress bar
                progressBar1.Maximum = InputFile.NumberOfTimeSteps - 1;
                progressBar1.Step = 1;
                //Loop over all time steps to get results for maxH (starting from 2nd time step)
                for (int j = 1; j < InputFile.NumberOfTimeSteps; j++)
                {
                    SourceDataH = (float[])InputFile.ReadItemTimeStep(ItemNbH + 1, j).Data; //Load the new time step H data into the SourceDataH array. ReadItemTimeStep is 1-based!
                    SourceDataU = (float[])InputFile.ReadItemTimeStep(ItemNbU + 1, j).Data; //Load the new time step U data into the SourceDataU array. ReadItemTimeStep is 1-based!
                    SourceDataV = (float[])InputFile.ReadItemTimeStep(ItemNbV + 1, j).Data; //Load the new time step V data into the SourceDataV array. ReadItemTimeStep is 1-based!
                    for (int k = 0; k < InputFile.NumberOfElements; k++) //Loop over all elements
                    {
                        if (SourceDataH[k] > OutputDataH[k]) //If the water depth for the new time step is higher than the previous maximum depth, then store the corresponding U, V, speed and direction values
                        {
                            OutputDataH[k] = SourceDataH[k];
                            OutputDataU[k] = SourceDataU[k];
                            OutputDataV[k] = SourceDataV[k];
                            OutputDataSpeed[k] = (float)Math.Sqrt(Math.Pow(SourceDataU[k], 2) + Math.Pow(SourceDataV[k], 2));
                            OutputDataDir[k] = (float)Math.Atan2(SourceDataU[k], SourceDataV[k]);
                        }
                    }
                    progressBar1.PerformStep(); //Increment progress bar
                }
                // Write results
                string folder = Path.GetDirectoryName(txtPath.Text);
                string FileRoot = Path.GetFileNameWithoutExtension(txtPath.Text);
                string FileNameOut = folder + "\\" + FileRoot + "_Statistics_Hmax.dfsu"; //Add suffix to input file name, to be used for output file
                DfsuFile OutputFile = OutputBuilder.CreateFile(FileNameOut); //Create output file
                OutputFile.WriteItemTimeStepNext(0, OutputDataH); //Write H data. Time set to 0 : ignored since equidistant interval
                OutputFile.WriteItemTimeStepNext(0, OutputDataU); //Write U data
                OutputFile.WriteItemTimeStepNext(0, OutputDataV); //Write V data
                OutputFile.WriteItemTimeStepNext(0, OutputDataSpeed); //Write speed data
                OutputFile.WriteItemTimeStepNext(0, OutputDataDir); //Write direction data
                InputFile.Close(); //Release the input file
                OutputFile.Close(); //Release the output file
                MessageBox.Show("File created"); //Confirm that the file has been created
                progressBar1.Value = 0; //Reset the progress bar
                btStart.Enabled = true; //Enable the Start button again
                btClose.Enabled = true; //Enable the Close button again
            }
        }

        private void ComputeVmax(string filename)
        {
            btStart.Enabled = false; //Disable the Start button, to prevent user from running the tool twice at the same time
            btClose.Enabled = false; //Disable the Close button, to prevent user from closing the tool while running
            //Read input file and find relevant items
            IDfsuFile InputFile = DfsuFile.Open(filename); //Open the file
            int ItemNb = InputFile.ItemInfo.Count; //Number of items in the file
            int ItemNbH = -1; //Stores the item number for water depth. Initialised with a temporary value.
            int ItemNbU = -1; //Stores the item number for U velocity. Initialised with a temporary value.
            int ItemNbV = -1; //Stores the item number for V velocity. Initialised with a temporary value.
            for (int i = 0; i < ItemNb; i++) //Loop finding appropriate items H, U and V
            {
                if (InputFile.ItemInfo[i].Name == "Total water depth") ItemNbH = i; //Save the actual item number when Total water depth is found
                if (InputFile.ItemInfo[i].Name == "U velocity") ItemNbU = i; //Save the actual item number when U velocity is found
                if (InputFile.ItemInfo[i].Name == "V velocity") ItemNbV = i; //Save the actual item number when V velocity is found
            }
            if (ItemNbH == -1 || ItemNbU == -1 || ItemNbV == -1) //If one of the required item cannot be found
            {
                btClose.Enabled = true; //Enable the Close button again
                throw new Exception("The result file doesn't contain the necessary items H, U and V"); //Throw error message
            }
            else
            {
                //Create output file, with same nodes, elements, projection and time info as the input file, but with different output items
                DfsuBuilder OutputBuilder = DfsuBuilder.Create(DfsuFileType.Dfsu2D);
                OutputBuilder.SetNodes(InputFile.X, InputFile.Y, InputFile.Z, InputFile.Code);
                OutputBuilder.SetElements(InputFile.ElementTable);
                OutputBuilder.SetProjection(InputFile.Projection);
                OutputBuilder.SetTimeInfo(InputFile.StartDateTime, InputFile.TimeStepInSeconds);
                OutputBuilder.SetZUnit(InputFile.ZUnit);
                OutputBuilder.AddDynamicItem("Maximum current speed", InputFile.ItemInfo[ItemNbU].Quantity); //Create item Speed
                OutputBuilder.AddDynamicItem("Water depth @ max. speed", InputFile.ItemInfo[ItemNbH].Quantity); //Create item H
                OutputBuilder.AddDynamicItem("U velocity @ max. speed", InputFile.ItemInfo[ItemNbU].Quantity); //Create item U
                OutputBuilder.AddDynamicItem("V velocity @ max. speed", InputFile.ItemInfo[ItemNbV].Quantity); //Create item V
                OutputBuilder.AddDynamicItem("Current direction @ max. speed", eumQuantity.Create(eumItem.eumICurrentDirection, eumUnit.eumUradian)); //Create item Direction. Note: eumQuantity requires "using DHI.Generic.MikeZero"
                //Initialization of all output variables. Both source data and output data are intialized with data from first time step of the input file.
                float[] SourceDataH = (float[])InputFile.ReadItemTimeStep(ItemNbH + 1, 0).Data; // ReadItemTimeStep is 1-based! That is, the first time step must be numbered 1, whereas the first time step in the file is numbered 0, hence the +1.
                float[] SourceDataU = (float[])InputFile.ReadItemTimeStep(ItemNbU + 1, 0).Data;
                float[] SourceDataV = (float[])InputFile.ReadItemTimeStep(ItemNbV + 1, 0).Data;
                float[] SourceDataSpeed = (float[])InputFile.ReadItemTimeStep(ItemNbU + 1, 0).Data;
                float[] OutputDataH = (float[])InputFile.ReadItemTimeStep(ItemNbH + 1, 0).Data;
                float[] OutputDataU = (float[])InputFile.ReadItemTimeStep(ItemNbU + 1, 0).Data;
                float[] OutputDataV = (float[])InputFile.ReadItemTimeStep(ItemNbV + 1, 0).Data;
                float[] OutputDataSpeed = (float[])InputFile.ReadItemTimeStep(ItemNbU + 1, 0).Data; // Initialise speed with values of U at time step 0
                float[] OutputDataDir = (float[])InputFile.ReadItemTimeStep(ItemNbU + 1, 0).Data; // Initialise direction with values of U at time step 0
                for (int m = 0; m < InputFile.NumberOfElements; m++) //Change speed and direction at first time step based on U and V values, with a loop over each element
                {
                    SourceDataSpeed[m] = (float)Math.Sqrt(Math.Pow(SourceDataU[m], 2) + Math.Pow(SourceDataV[m], 2));
                    OutputDataSpeed[m] = SourceDataSpeed[m];
                    OutputDataDir[m] = (float)Math.Atan2(SourceDataU[m], SourceDataV[m]);
                }
                //Define the properties of the progress bar
                progressBar1.Maximum = InputFile.NumberOfTimeSteps - 1;
                progressBar1.Step = 1;
                //Loop over all time steps to get results for maxV (starting from 2nd time step)
                for (int j = 1; j < InputFile.NumberOfTimeSteps; j++)
                {
                    SourceDataH = (float[])InputFile.ReadItemTimeStep(ItemNbH + 1, j).Data; //Load the new time step H data into the SourceDataH array. ReadItemTimeStep is 1-based!
                    SourceDataU = (float[])InputFile.ReadItemTimeStep(ItemNbU + 1, j).Data; //Load the new time step U data into the SourceDataU array. ReadItemTimeStep is 1-based!
                    SourceDataV = (float[])InputFile.ReadItemTimeStep(ItemNbV + 1, j).Data; //Load the new time step V data into the SourceDataV array. ReadItemTimeStep is 1-based!
                    for (int k = 0; k < InputFile.NumberOfElements; k++) //Loop over all elements
                    {
                        SourceDataSpeed[k] = (float)Math.Sqrt(Math.Pow(SourceDataU[k], 2) + Math.Pow(SourceDataV[k], 2)); //Compute speed based on U and V
                        if (SourceDataSpeed[k] > OutputDataSpeed[k]) //If the current speed for the new time step is higher than the previous maximum speed, then store the corresponding H, U, V and direction values
                        {
                            OutputDataSpeed[k] = SourceDataSpeed[k];
                            OutputDataH[k] = SourceDataH[k];
                            OutputDataU[k] = SourceDataU[k];
                            OutputDataV[k] = SourceDataV[k];
                            OutputDataDir[k] = (float)Math.Atan2(SourceDataU[k], SourceDataV[k]);
                        }
                    }
                    progressBar1.PerformStep(); //Increment progress bar
                }
                // Write results
                string folder = Path.GetDirectoryName(txtPath.Text);
                string FileRoot = Path.GetFileNameWithoutExtension(txtPath.Text);
                string FileNameOut = folder + "\\" + FileRoot + "_Statistics_Vmax.dfsu"; //Add suffix to input file name, to be used for output file
                DfsuFile OutputFile = OutputBuilder.CreateFile(FileNameOut); //Create output file
                OutputFile.WriteItemTimeStepNext(0, OutputDataSpeed); //Write Speed data. Time set to 0 : ignored since equidistant interval
                OutputFile.WriteItemTimeStepNext(0, OutputDataH); //Write H data
                OutputFile.WriteItemTimeStepNext(0, OutputDataU); //Write U data
                OutputFile.WriteItemTimeStepNext(0, OutputDataV); //Write V data
                OutputFile.WriteItemTimeStepNext(0, OutputDataDir); //Write direction data
                InputFile.Close(); //Release the input file
                OutputFile.Close(); //Release the output file
                MessageBox.Show("File created"); //Confirm that the file has been created
                progressBar1.Value = 0; //Reset the progress bar
                btStart.Enabled = true; //Enable the Start button again
                btClose.Enabled = true; //Enable the Close button again
            }
        }
    }
}
