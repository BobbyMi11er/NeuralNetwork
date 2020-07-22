package mnist;
import java.io.*;

public class mnist_processor {
    
    public static void main(String[]args) throws IOException {
        File training = new File("mnist_train.csv");
        FileInputStream in = new FileInputStream(training);
        byte[] contents = new byte[(int)training.length()];
        in.read(contents);
        in.close();
        for (int i = 0;i < 100; i++) {
            System.out.print(contents[i] + " ");
        }
    }
}