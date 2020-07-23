import java.io.*;
import java.util.*;

public class data_formatter {
    static String FILEPATH = "C:\\Users\\GuestUser\\Downloads\\";
    static String InputLine = "";
    static int ROWS = 0, COLS = 0;
    static ArrayList<ArrayList<Integer>> inputs; 
    static ArrayList<Integer> answers;

    public data_formatter(String path, int approx_rows, int approx_cols) {
        // assumes file is in downloads
        FILEPATH += path;
        ROWS = approx_rows;
        COLS = approx_cols;
    }
    public data_formatter(String path) {
        FILEPATH += path;
    }

    public void format() {
        File file = new File(FILEPATH);
        try {
            Scanner scanIn = new Scanner(new BufferedReader(new FileReader(file)));
            if (ROWS != 0) {
                answers = new ArrayList<>(ROWS);
                inputs = new ArrayList<>(COLS);
            }
            else {
                answers = new ArrayList<>();
                inputs = new ArrayList<>();
            }
            int k = 0;
            while(scanIn.hasNextLine()) {
                InputLine = scanIn.nextLine();
                String [] InArray = InputLine.split(",");
                inputs.add(new ArrayList<Integer>());
                
                // answer is first integer in row
                answers.add(Integer.parseInt(InArray[0]));
                
                for (int i = 1; i < InArray.length; i ++) {
                    inputs.get(k).add(Integer.parseInt(InArray[i]));
                }
                k ++;
            }
        }
        catch (FileNotFoundException e) {
            System.out.println("NO FILE FOUND");
        }
    }

    public void printData() {
        for (int i = 0; i < inputs.size(); i ++) {
            for (int j = 0; j < inputs.get(i).size(); j ++) {
                System.out.print(inputs.get(i).get(j) + " ");
            }
            System.out.println("");
        }
    }
    
    public static void main(String[]args) throws IOException {
        data_formatter formatter = new data_formatter("csv_test.csv");
        formatter.format();
        formatter.printData();
    }
} 