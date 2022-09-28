using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace TransactionsApps
{
  public class Pembelian
  {
    public DateTime tanggal { get; set; }
  }

  class Program
  {
    public string menu, command;

    static void Main(string[] args)
    {
      Program p = new Program();
      if (!File.Exists("pembelian.csv"))
      {
        p.createCsv();
      }

      while (p.menu != "exit" && p.command != "exit" )
      {
        if (p.command == "" || p.command == null || p.command == "menu")
        {
          p.initMenu();
        } else
        {
          p.initCommand();
        }
      }     
    }

    public async void createCsv()
    {
      string[] pembelianHeader =
        {
            "ID; Tanggal; Keterangan Pembelian; Sebesar"
        };

      await File.WriteAllLinesAsync("pembelian.csv", pembelianHeader);

      string[] penjualanHeader =
        {
            "ID; Tanggal; Keterangan Penjualan; Sebesar"
        };

      await File.WriteAllLinesAsync("penjualan.csv", penjualanHeader);
    }

    public void initMenu()
    {
      Console.Write("\nMenu (pembelian|penjualan|exit): ");
      menu = Console.ReadLine();
      if (menu == "pembelian" || menu == "penjualan")
      {
        initCommand();
      } else if (menu == "exit")
      {

      } else
      {
        Console.Write("\nMenu tidak tersedia. Silahkan coba kembali.\n");
        initMenu();
      }
    }

    public void initCommand()
    {
      Console.Write($"\nPerintah pada menu {menu} (create|read|update|delete|menu|exit): ");
      command = Console.ReadLine();
      if (command == "create" || command == "read" || command == "update" || command == "delete")
      {
        Console.Write("\n");
        switch (command)
        {
          case "read":
            read(menu);
            break;
          case "create":
            create(menu);
            break;
          case "update":
            update(menu);
            break;
          default:
            break;
        }
      }
      else if (command == "menu" || command == "exit")
      {

      }
      else
      {
        Console.Write("\nPerintah tidak tersedia. Silahkan coba kembali.\n");
        initCommand();
      }
    }

    public void read(string table)
    {
      var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
      {
        HasHeaderRecord = false,
        Comment = '#',
        AllowComments = true,
        Delimiter = ";",
      };

      using var streamReader = File.OpenText($"{table}.csv");
      using var csvReader = new CsvReader(streamReader, csvConfig);

      while (csvReader.Read())
      {
        var ID = csvReader.GetField(0);
        var tanggal = csvReader.GetField(1);
        var keterangan = csvReader.GetField(2);
        var sebesar = csvReader.GetField(3);

        Console.WriteLine($"| {ID} | {tanggal} | {keterangan} | {sebesar}");
      }
    }

    public string readID(string table)
    {
      string streamReader = File.ReadLines($"{table}.csv").Last();
      string ID = streamReader.Split(";")[0];
      return ID;
    }

    public string keterangan;
    public int sebesar;

    public void create(string table)
    {
      Console.Write($"Keterangan {table} (maksimal 50 karakter): ");
      keterangan = Console.ReadLine();

      if (keterangan.Length <= 50)
      {
        createSebesar(table);
      } else
      {
        Console.Write($"\nKeterangan {table} tidak boleh melebihi 50 karakter.\n");
        create(table);
      }
    }

    public void createSebesar(string table)
    {
      Console.Write($"\nSebesar (harus angka dan lebih dari 0): ");

      if (int.TryParse(Console.ReadLine(), out sebesar))
      {
        int ID = Convert.ToInt32(readID(table)) + 1;
        string tanggal = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");        
        string row = $"{ID}; {tanggal}; {keterangan}; {sebesar}\n";

        File.AppendAllText($"{table}.csv", row);

        Console.WriteLine("\nPencatatan berhasil disimpan.\n");
        read(table);
      }
      else
      {
        Console.Write($"\nSebesar tidak valid atau bernilai 0. silahkan coba kembali\n");
        createSebesar(table);
      }
    }

    public void update(string table)
    {
      Console.Write($"Keterangan {table} (maksimal 50 karakter): ");
      keterangan = Console.ReadLine();

      if (keterangan.Length <= 50)
      {
        createSebesar(table);
      }
      else
      {
        Console.Write($"\nKeterangan {table} tidak boleh melebihi 50 karakter.\n");
        create(table);
      }
    }
  }
}
