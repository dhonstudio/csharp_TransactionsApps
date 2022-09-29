using System;
using System.IO;

namespace TransactionsApps
{
  class Program
  {
    private readonly TransaksiCRUDRepository transaksiCRUDRepository = new();
    private readonly TransaksiSortRepository transaksiSortRepository = new();
    private readonly TransaksiSearchRepository transaksiSearchRepository = new();
    private string menu, command;

    static void Main(string[] args)
    {
      Program p = new();
      if (!File.Exists("pembelian.csv"))
      {
        CreateCsv();
      }

      while (p.menu != "exit" && p.command != "exit" )
      {
        if (p.command == "" || p.command == null || p.command == "menu")
        {
          p.InitMenu();
        } else
        {
          p.InitCommand();
        }
      }     
    }

    private static async void CreateCsv()
    {
      string[] pembelianHeader =
        {
            "ID;Tanggal;Keterangan Pembelian;Sebesar",
            "1;01-09-2022 07:01:28;Bahan Baku;10000000",
            "2;01-09-2022 07:15:15;Bahan Pengemas;5000000",
            "3;02-09-2022 08:11:34;Bahan Baku Tambahan;2000000"
        };

      await File.WriteAllLinesAsync("pembelian.csv", pembelianHeader);

      string[] penjualanHeader =
        {
            "ID;Tanggal;Keterangan Penjualan;Sebesar",
            "1;01-09-2022 13:01:28;Produk A;15000000",
            "2;01-09-2022 13:14:36;Produk B;2000000",
            "3;01-09-2022 15:17:50;Produk C;1000000"
        };

      await File.WriteAllLinesAsync("penjualan.csv", penjualanHeader);
    }

    private void InitMenu()
    {
      Console.Write("\nMenu (pembelian|penjualan|report|exit): ");
      menu = Console.ReadLine();
      if (menu == "pembelian" || menu == "penjualan")
      {
        InitCommand();
      } else if (menu == "report")
      {
        transaksiCRUDRepository.Report();
      } else if (menu == "exit")
      {

      } else
      {
        Console.Write("\nMenu tidak tersedia. Silahkan coba kembali.\n");
        InitMenu();
      }
    }

    private void InitCommand()
    {
      Console.Write($"\nPerintah pada menu {menu} (create|read|update|delete|sort|search|menu|exit): ");
      command = Console.ReadLine();
      if (command == "create" || command == "read" || command == "update" || command == "delete" || command == "sort" || command == "search")
      {
        Console.Write("\n");
        switch (command)
        {
          case "create":
            transaksiCRUDRepository.Create(menu);
            break;
          case "read":
            transaksiCRUDRepository.Read(menu);
            break;          
          case "update":
            transaksiCRUDRepository.Update(menu);
            break;
          case "delete":
            transaksiCRUDRepository.Delete(menu);
            break;

          case "sort":
            transaksiSortRepository.Sort(menu);
            break;
          case "search":
            transaksiSearchRepository.Search(menu);
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
        InitCommand();
      }
    }
  }
}
