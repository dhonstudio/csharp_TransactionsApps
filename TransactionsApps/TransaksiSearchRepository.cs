using System;
using System.Collections.Generic;
using System.Linq;

namespace TransactionsApps
{
  class TransaksiSearchRepository
  {
    private List<Transaksi> Datas;
    private readonly TransaksiCRUDRepository transaksiCRUDRepository = new();

    public void Search(string table)
    {
      Console.Write($"Masukkan keyword: ");
      var keyword = Console.ReadLine();

      Datas = transaksiCRUDRepository.ReadDatas(table);
      Transaksi DatasCopy = Datas[0];
      Datas.RemoveAt(0);
      Datas = Datas.Where(x => x.ID.ToLower() == keyword.ToLower() || x.tanggal.ToLower() == keyword.ToLower() || x.keterangan.ToLower() == keyword.ToLower() || x.sebesar.ToLower() == keyword.ToLower()).ToList();

      Console.WriteLine($"\n| {DatasCopy.ID} | {DatasCopy.tanggal} | {DatasCopy.keterangan} | {DatasCopy.sebesar} |");

      for (int i = 0; i < Datas.Count; i++)
      {
        Console.WriteLine($"| {Datas[i].ID} | {Datas[i].tanggal} | {Datas[i].keterangan} | {string.Format("{0:#,0}", Convert.ToInt32(Datas[i].sebesar))} |");
      }
    }
  }
}