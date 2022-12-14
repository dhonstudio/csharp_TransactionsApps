using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

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

    private CsvConfiguration Configuration() {
      var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
      {
        HasHeaderRecord = false,
        Comment = '#',
        AllowComments = true,
        Delimiter = ";",
      };

      return csvConfig;
    }

    public List<Transaksi> ReadDatas(string table)
    {
      var csvConfig = Configuration();
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

    public void Read(string table)
    {
      Datas = ReadDatas(table);

      for (int i = 0; i < Datas.Count; i++)
      {
        if (i == 0)
        {
          Console.WriteLine($"| {Datas[i].ID} | {Datas[i].tanggal} | {Datas[i].keterangan} | {Datas[i].sebesar} |");
        } else
        {
          Console.WriteLine($"| {Datas[i].ID} | {Datas[i].tanggal} | {Datas[i].keterangan} | {string.Format("{0:#,0}", Convert.ToInt32(Datas[i].sebesar))} |");
        }        
      }      
    }

    private List<string> ReadIDs(string table)
    {
      var csvConfig = Configuration();
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

    public void Report()
    {
      var DataPembelian = ReadDatas("pembelian");
      var DataPenjualan = ReadDatas("penjualan");

      var reports = new List<Report>();
      var report = new Report();

      for (int i = 1; i < DataPembelian.Count; i++)
      {
        report = new Report()
        {
          ID = "b" + DataPembelian[i].ID,
          tanggal = DataPembelian[i].tanggal,
          keterangan = DataPembelian[i].keterangan,
          debet = 0.ToString(),
          kredit = DataPembelian[i].sebesar
        };
        reports.Add(report);
      }

      for (int i = 1; i < DataPenjualan.Count; i++)
      {
        report = new Report()
        {
          ID = "a" + DataPenjualan[i].ID,
          tanggal = DataPenjualan[i].tanggal,
          keterangan = DataPenjualan[i].keterangan,
          debet = DataPenjualan[i].sebesar,
          kredit = 0.ToString()
        };
        reports.Add(report);
      }

      reports = reports.OrderBy(q => q.tanggal).ToList();

      var finalReports = new List<Report>();
      var finalReport = new Report();

      for (int i = 0; i < reports.Count; i++)
      {
        finalReport = reports[i];
        if (i == 0) {          
          finalReport.laba = (Convert.ToInt32(reports[i].debet) - Convert.ToInt32(reports[i].kredit)).ToString();
          finalReports.Add(finalReport);
        } else
        {
          finalReport.laba = (Convert.ToInt32(finalReports.Last().laba) + Convert.ToInt32(reports[i].debet) - Convert.ToInt32(reports[i].kredit)).ToString();
          finalReports.Add(finalReport);
        }
      }

      string[] reportHeader =
        {
            "ID;Tanggal;Keterangan;Debet;Kredit;Saldo Laba"
        };

      Console.WriteLine($"\n| ID | Tanggal | Keterangan | Debet | Kredit | Saldo Laba |");
      File.WriteAllLines("report.csv", reportHeader);

      for (int i = 0; i < finalReports.Count; i++)
      {
        Console.WriteLine($"| {finalReports[i].ID} | {finalReports[i].tanggal} | {finalReports[i].keterangan} | {string.Format("{0:#,0}", Convert.ToInt32(finalReports[i].debet))} | {string.Format("{0:#,0}", Convert.ToInt32(finalReports[i].kredit))} | {string.Format("{0:#,0}", Convert.ToInt32(finalReports[i].laba))} |");
        File.AppendAllText("report.csv", $"{finalReports[i].ID};{finalReports[i].tanggal};{finalReports[i].keterangan};{string.Format("{0:#,0}", Convert.ToInt32(finalReports[i].debet))};{string.Format("{0:#,0}", Convert.ToInt32(finalReports[i].kredit))};{string.Format("{0:#,0}", Convert.ToInt32(finalReports[i].laba))}\n");
      }
    }
  }
}