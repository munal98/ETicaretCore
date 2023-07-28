using AppCore.Business.Models.Ordering;
using AppCore.Business.Models.Paging;
using AppCore.Business.Results;
using AppCore.Business.Services.Bases;
using Business.Models;
using Business.Models.Filters;
using Business.Models.Reports;

namespace Business.Services.Bases
{
    public interface IUrunService : IService<UrunModel>
    {
        Result<List<UrunRaporuModel>> UrunRaporuGetir(UrunRaporuFiltreModel filtre = null, PageModel sayfa = null, OrderModel sira = null);
    }
}
