using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSecurity.Enums;

namespace WebSecurity.Interfaces;

public interface ICrsRepository<TModel, TSettings>
{
    TSettings GetSettings();
    TModel GetById(IdType idType, int id);
    IEnumerable<TModel> GetAll();
    void Add(TModel entity);
    void Update(TModel entity);
    void Delete(TModel entity);
}

public interface ICrsRepository<TModel>
{
    TModel GetById(IdType idType, int id);
    IEnumerable<TModel> GetAll();
    void Add(TModel entity);
    void Update(TModel entity);
    void Delete(TModel entity);
}
