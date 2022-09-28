using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionsApps
{
  class TransaksiCRUDRepository
  {
    private string ID, keterangan;
    private int sebesar, index;
    private List<Transaksi> Datas;

    public void Create(string table)
    {
      CreateKeterangan(table);
    }

    private void CreateKeterangan(string table)
    {
      Console.Write($"Keterangan {table} (maksimal 50 karakter): ");
      keterangan = Console.ReadLine();

      if (keterangan.Length > 0 && keterangan.Length <= 50)
      {
        CreateSebesar(table);
      }
      else
      {
        Console.Write($"\nKeterangan {table} harus diisi dan tidak boleh melebihi 50 karakter.\n\n");
        CreateKeterangan(table);
      }
    }

    private static int ReadID(string table)
    {
      var lines = File.ReadLines($"{table}.csv");
      string IdTrx = "";
      if (lines.Count() > 1)
      {
        string streamReader = lines.Last();
        IdTrx = streamReader.Split(";")[0];
      } else
      {
        IdTrx = "0";
      }
      return Convert.ToInt32(IdTrx);
    }

    private void CreateSebesar(string table)
    {
      Console.Write($"\nSebesar (harus angka dan lebih dari 0): ");
      var sebesarInput = Console.ReadLine();
      if (int.TryParse(sebesarInput, out sebesar))
      {
        if (Convert.ToInt32(sebesarInput) > 0)
        {
          ID = (ReadID(table) + 1).ToString();
          string tanggal = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
          string row = $"{ID};{tanggal};{keterangan};{sebesar}\n";

          File.AppendAllText($"{table}.csv", row);

          Console.WriteLine("\nPencatatan berhasil disimpan.\n");
          Read(table);
        }
        else
        {
          Console.Write($"\nSebesar tidak dapat bernilai 0. silahkan coba kembali\n");
          CreateSebesar(table);
        }
      }
      else
      {
        Console.Write($"\nSebesar tidak valid. silahkan coba kembali\n");
        CreateSebesar(table);
      }
    }

    public void Read(string table)
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

      List<Transaksi> datas = new List<Transaksi>();
      Transaksi data = new();
      while (csvReader.Read())
      {
        data = new Transaksi()
        {
          ID = csvReader.GetField(0),
          tanggal = csvReader.GetField(1),
          keterangan = csvReader.GetField(2),
          sebesar = csvReader.GetField(3)
        };
        datas.Add(data);
      }

      for (int i = 0; i < datas.Count; i++)
      {
        if (i == 0)
        {
          Console.WriteLine($"| {datas[i].ID} | {datas[i].tanggal} | {datas[i].keterangan} | {datas[i].sebesar} |");
        } else
        {
          Console.WriteLine($"| {datas[i].ID} | {datas[i].tanggal} | {datas[i].keterangan} | {string.Format("{0:#,0}", Convert.ToInt32(datas[i].sebesar))} |");
        }        
      }      
    }

    private static List<Transaksi> ReadDatas(string table)
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

      var Data = new Transaksi();
      var Datas = new List<Transaksi>();

      while (csvReader.Read())
      {
        Data = new Transaksi
        {
          ID = csvReader.GetField(0),
          tanggal = csvReader.GetField(1),
          keterangan = csvReader.GetField(2),
          sebesar = csvReader.GetField(3)
        };
        Datas.Add(Data);
      }

      return Datas;
    }

    private static List<string> ReadIDs(string table)
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

      var IDs = new List<string>();

      while (csvReader.Read())
      {
        var ID = csvReader.GetField(0);
        IDs.Add(ID);
      }

      return IDs;
    }

    public void Update(string table)
    {
      Console.Write($"ID yang akan diupdate: ");
      ID = Console.ReadLine();

      var IDs = ReadIDs(table);

      if (IDs.Contains(ID))
      {
        UpdateKeterangan(table);
      }
      else
      {
        Console.Write($"\nID tidak ditemukan.\n\n");
        Update(table);
      }
    }

    private void UpdateKeterangan(string table)
    {
      Console.Write($"\nKeterangan {table} (kosongkan apabila tidak ingin diubah): ");
      keterangan = Console.ReadLine();

      if (keterangan.Length <= 50)
      {
        Datas = ReadDatas(table);

        index = Datas.FindIndex(x => x.ID == ID);
        if (keterangan.Length == 0)
        {
          keterangan = Datas[index].keterangan;
        }
        UpdateSebesarAsync(table);
      }
      else
      {
        Console.Write($"\nKeterangan {table} tidak boleh melebihi 50 karakter.\n");
        UpdateKeterangan(table);
      }
    }

    private void UpdateSebesarAsync(string table)
    {
      Console.Write($"\nSebesar (kosongkan apabila tidak ingin diubah): ");
      var sebesarInput = Console.ReadLine();

      if (sebesarInput == "" || int.TryParse(sebesarInput, out sebesar))
      {
        if (sebesarInput == "" || Convert.ToInt32(sebesarInput) > 0)
        {
          if (sebesarInput == "")
          {
            sebesarInput = Datas[index].sebesar;
          }

          string tanggal = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
          List<string> rows = new List<string>();
          for (int i = 0; i < Datas.Count; i++)
          {
            if (i == index)
            {
              rows.Add($"{Datas[i].ID};{tanggal};{keterangan};{sebesarInput}");              
            }
            else
            {
              rows.Add($"{Datas[i].ID};{Datas[i].tanggal};{Datas[i].keterangan};{Datas[i].sebesar}");
            }
          }

          File.WriteAllLines($"{table}.csv", rows.ToArray());

          Console.WriteLine("\nPencatatan berhasil diubah.\n");
          Read(table);
        }
        else
        {
          Console.Write($"\nSebesar tidak dapat bernilai 0. silahkan coba kembali\n");
          UpdateSebesarAsync(table);
        }
      }
      else
      {
        Console.Write($"\nSebesar tidak valid. silahkan coba kembali\n");
        UpdateSebesarAsync(table);
      }
    }

    public void Delete(string table)
    {
      Console.Write($"ID yang akan dihapus: ");
      ID = Console.ReadLine();

      var IDs = ReadIDs(table);

      if (IDs.Contains(ID))
      {
        Datas = ReadDatas(table);
        index = Datas.FindIndex(x => x.ID == ID);
        List<string> rows = new List<string>();
        for (int i = 0; i < Datas.Count; i++)
        {
          if (i != index)
          {
            rows.Add($"{Datas[i].ID};{Datas[i].tanggal};{Datas[i].keterangan};{Datas[i].sebesar}");
          }
        }

        File.WriteAllLines($"{table}.csv", rows.ToArray());

        Console.WriteLine($"\nID {ID} berhasil dihapus.\n");
        Read(table);
      }
      else
      {
        Console.Write($"\nID tidak ditemukan.\n\n");
        Update(table);
      }
    }
  }
}