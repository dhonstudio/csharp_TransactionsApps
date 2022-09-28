using System;
using System.Collections.Generic;
using System.Linq;

namespace TransactionsApps
{
  class GFG : IComparer<int>
  {
    public int Compare(int x, int y)
    {
      if (x == 0 || y == 0)
      {
        return 0;
      }

      return x.CompareTo(y);
    }
  }

  class TransaksiSortRepository
  {
    private List<Transaksi> Datas;
    private readonly TransaksiCRUDRepository transaksiCRUDRepository = new();

    public void Sort(string table)
    {
      Console.Write($"Akan disort berdasarkan apa (id|tanggal|keterangan|sebesar): ");
      var sortby = Console.ReadLine();
      Console.Write($"\n(asc|desc): ");
      var sortmethod = Console.ReadLine();

      Datas = transaksiCRUDRepository.ReadDatas(table);
      Transaksi DatasCopy = Datas[0];
      Datas.RemoveAt(0);
      switch (sortby)
      {
        case "tanggal":
          if (sortmethod == "desc")
          {
            Datas = Datas.OrderByDescending(q => q.tanggal).ToList();
          } else
          {
            Datas = Datas.OrderBy(q => q.tanggal).ToList();
          }
          break;
        case "keterangan":
          if (sortmethod == "desc")
          {
            Datas = Datas.OrderByDescending(q => q.keterangan).ToList();
          }
          else
          {
            Datas = Datas.OrderBy(q => q.keterangan).ToList();
          }
          break;
        case "sebesar":
          if (sortmethod == "desc")
          {
            Datas = Datas.OrderByDescending(q => q.sebesar).ToList();
          }
          else
          {
            Datas = Datas.OrderBy(q => q.sebesar).ToList();
          }
          break;
        default:
          if (sortmethod == "desc")
          {
            Datas = Datas.OrderByDescending(q => q.ID).ToList();
          }
          else
          {
            Datas = Datas.OrderBy(q => q.ID).ToList();
          }
          break;
      }

      Console.WriteLine($"\n| {DatasCopy.ID} | {DatasCopy.tanggal} | {DatasCopy.keterangan} | {DatasCopy.sebesar} |");

      for (int i = 0; i < Datas.Count; i++)
      {
        Console.WriteLine($"| {Datas[i].ID} | {Datas[i].tanggal} | {Datas[i].keterangan} | {string.Format("{0:#,0}", Convert.ToInt32(Datas[i].sebesar))} |");
      }
    }
  }
}