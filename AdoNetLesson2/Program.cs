using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetLesson2
{
    class Program
    {
        
        static SalesDBConnect salesDb = new SalesDBConnect();
        static string DateTimeByKeyboard()
        {
            byte day=1;
            byte month=1;
            int year=1;
            
            do
            {
                Console.Clear();
                Console.WriteLine("Enter day of sell: ");
            }
            while (!getAnswer(ref day));
            do
            {
                Console.Clear();
                Console.WriteLine("Enter month of sell: ");
            }
            while (!getAnswer(ref month));
            do
            {
                Console.Clear();
                Console.WriteLine("Enter year of sell: ");
            }
            while (!getAnswer(ref year));
            
            return $"{day}-{month}-{year}";
        }
        static void addNewSell()
        {
            Console.Clear();
            int buyer_id = 0;
            int seller_id = 0;
            float sum = 0;
           
            do
            {
                Console.Clear();
                Console.WriteLine("Enter buyer id: ");
            }
            while (!getAnswer(ref buyer_id));
            do
            {
                Console.Clear();
                Console.WriteLine("Enter seller id: ");
            }
            while (!getAnswer(ref seller_id));
            do
            {
                Console.Clear();
                Console.WriteLine("Enter sum of sell: ");
            }
            while (!getAnswer(ref sum));
            
            try {
                string sell_date = DateTimeByKeyboard();

                SqlCommand command = new SqlCommand("insert into Sales(SellerId,BuyerId,Price,SellDate) values(@seller_id,@buyer_id,@sum,@sell_date)", SalesDBConnect.connection);
                command.Parameters.AddWithValue("@seller_id",seller_id);
                command.Parameters.AddWithValue("@buyer_id", buyer_id);
                command.Parameters.AddWithValue("@sum", sum);
                command.Parameters.AddWithValue("@sell_date", sell_date);
                
                command.ExecuteNonQuery();
                Console.WriteLine("Added");
                Console.ReadKey();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
        static void printMenu()
        {
            Console.WriteLine("=============================================================");
            Console.WriteLine("1. Add new Sale/Buy");
            Console.WriteLine("2. Show Info about sales by period");
            Console.WriteLine("3. Show last buy of Buyer by period");
            Console.WriteLine("4. Delete buyer or seller by id");
            Console.WriteLine("5. Shown to sellers with the highest total sales");
            Console.WriteLine("0. Exit");
            Console.WriteLine("=============================================================");
        }
        static bool getAnswer(ref byte answer) {
            try
            {
                answer = Byte.Parse(Console.ReadLine());
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Enter correct answer");
                return false;
            }
        }
        static bool getAnswer(ref DateTime answer)
        {
            try
            {
                answer = DateTime.Parse(Console.ReadLine());
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Enter correct answer");
                return false;
            }
        }
        static bool getAnswer(ref float answer)
        {
            try
            {
                answer = float.Parse(Console.ReadLine());
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Enter correct answer");
                return false;
            }
        }
        static bool getAnswer(ref int answer)
        {
            try
            {
                answer = Int32.Parse(Console.ReadLine());
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Enter correct answer");
                return false;
            }
        }
        
        static void Main(string[] args)
        {
            bool isExit = false;
            byte answer = 0;
            while (!isExit)
            {
                Console.Clear();
                printMenu();
                while (!getAnswer(ref answer))
                {
                    Console.Clear();
                    printMenu();
                }
                switch (answer)
                {
                    case 0:
                        Console.WriteLine("Goodbye");
                        return;
                    case 1:
                        addNewSell();
                        break;
                    case 2:
                        ShowSellsByPeriod();
                        break;
                    case 3:
                        ShowLastBuyByNameAndSurname();
                        break;
                    case 4:
                        DeleteBuyerOrSellerById();
                        break;
                    case 5:
                        ShowSellerWithBiggerTotalSummOfSells();
                        break;
                    default:
                        break;
                }
            }
            SalesDBConnect.connection.Close();
        }

        private static void ShowSellerWithBiggerTotalSummOfSells()
        {
            //Тут я заюзав процедуру, так як тут немає параметрів

            // -- SQL Query --
            //create procedure ShowSellerWithBiggerTotalSummOfSells
            //as
            //select top 1 se.Name, se.Surname from Sales as s join Sellers as se on s.SellerId = se.Id group by se.Id,se.Name,se.Surname order by Sum(s.Price) desc


            SqlCommand command = new SqlCommand("ShowSellerWithBiggerTotalSummOfSells", SalesDBConnect.connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataReader reader = command.ExecuteReader();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write(reader.GetName(i) + "\t");
            }
            Console.WriteLine("\n-------------------------------------------------------------------------------------");
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader[i] + "\t");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
            reader.Close();
        }

        private static void DeleteBuyerOrSellerById()
        {
            byte answer = 0;
            byte id = 1;
            do
            {
                Console.Clear();
                Console.WriteLine($"1. Buyer");
                Console.WriteLine($"2. Seller");
            }
            while (!getAnswer(ref answer));
            switch (answer)
            {
                case 1:
                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"1. EnterId");
                    }
                    while (!getAnswer(ref id));


                    SqlCommand command = new SqlCommand("delete b from Buyers as b where Id = @Id", SalesDBConnect.connection);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                    break;
                case 2:
                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"1. EnterId");
                    }
                    while (!getAnswer(ref id));
                    SqlCommand command2 = new SqlCommand("delete b from Sellers as b where Id = @Id", SalesDBConnect.connection);
                    command2.Parameters.AddWithValue("@Id", id);
                    command2.ExecuteNonQuery();
                    break;
                default:
                    break;
            }

        }

        private static void ShowLastBuyByNameAndSurname()
        {
            string b_name = Console.ReadLine();
            string b_surname = Console.ReadLine();

            SqlCommand command = new SqlCommand("select s.Id, s.Price from Sales as s join Buyers as b on s.BuyerId = b.Id where b.Name = @b_name and b.Surname = @b_surname", SalesDBConnect.connection);
            command.Parameters.AddWithValue("@b_name", b_name);
            command.Parameters.AddWithValue("@b_surname", b_surname);

            SqlDataReader reader = command.ExecuteReader();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write(reader.GetName(i) + "\t");
            }
            Console.WriteLine("\n-------------------------------------------------------------------------------------");
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader[i] + "\t");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
            reader.Close();
        }

        private static void ShowSellsByPeriod()
        {
            string first_date;
            string second_date;
            SqlDataReader reader;
            try
            {
                first_date = DateTimeByKeyboard();
                second_date = DateTimeByKeyboard();
                SqlCommand command = new SqlCommand("select * from Sales as s where s.SellDate between @first_date and @second_date", SalesDBConnect.connection);
                command.Parameters.AddWithValue("@first_date", first_date);
                command.Parameters.AddWithValue("@second_date", second_date);
               
                reader = command.ExecuteReader();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader.GetName(i) + "\t");
                }
                Console.WriteLine("\n-------------------------------------------------------------------------------------");
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader[i] + "\t");
                    }
                    Console.WriteLine();
                }
                Console.ReadKey();
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            
        }
    }
}
