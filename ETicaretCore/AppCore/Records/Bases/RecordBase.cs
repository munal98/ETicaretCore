﻿namespace AppCore.Records.Bases
{
    public class RecordBase
    {
        public int Id { get; set; }
        public string? Guid { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
