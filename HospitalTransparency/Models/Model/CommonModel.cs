using System.Collections.Generic;

namespace HospitalTransparency.Models.Model
{
    public class PaginationModel<T>
    {
        public List<T> Data { get; set; }
        public int Total { get; set; }
    }

    public class FileModel {
        public bool Flag { get; set; }
        public string Messgae { get; set; }
        public Filecls FileData { get; set; }
    }
    public class Filecls
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
    }

    public class keyValue {
        public long Key { get; set; }
        public string Value { get; set; }
        public string Display { get; set; }
    }
}