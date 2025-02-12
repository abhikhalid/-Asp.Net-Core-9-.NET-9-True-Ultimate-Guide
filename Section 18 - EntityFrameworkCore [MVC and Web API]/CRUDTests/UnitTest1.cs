namespace CRUDTests
{
    public class UnitTest1
    {
        [Fact] // Fact means, you are goint to write one or two unit test in this method
        public void Test1()
        {
            //Arrange
            // means, the declaration of variables and collecting the inputs
            MyMath myMath = new MyMath();
            int input1 = 10, input2 = 5;
            int expected = 15;

            //Act
            //act means, calling the method, which method you would like to test
            int actual =  myMath.Add(input1,input2);

            //Assert
            //means comparing the expected value with the actual value.
            //if the expected and actual value are same then test case is pass otherwise it is fail
            Assert.Equal(expected,actual);
        }
    }
}