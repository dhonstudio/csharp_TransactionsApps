namespace TransactionsApps
{
  public class Transaksi
  {
    public string ID { get; set; }
    public string tanggal { get; set; }
    public string keterangan { get; set; }
    public string sebesar { get; set; }
  }

  public class Report
  {
    public string ID { get; set; }
    public string tanggal { get; set; }
    public string keterangan { get; set; }
    public string debet { get; set; }
    public string kredit { get; set; }
    public string laba { get; set; }
  }
}