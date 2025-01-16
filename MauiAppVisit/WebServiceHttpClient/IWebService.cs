using MauiAppVisit.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppVisit.WebServiceHttpClient
{
    public interface IWebService
    {
        Task<string> GetInformationPlaceVRByIdPlace(int id);
        Task<ObservableCollection<Lugar>> CarregaLugaresAsync();
        Task<ObservableCollection<TypePlace>> CarregaTipoDeLugaresAsync();
        Task<List<Lugar>> GetLocationDetailsById(int id);
        Task<Stream> RequestDownloadFileVR(int id);
        Task<FileVrDetails> RequestFileVrDetails(int id);
        Task<UserToken> RegisterAsync(Usuario usuario);
        Task<UserToken> LogarAsync(Usuario usuario);
        Task<Usuario> GetUser(int id);
    }
}
