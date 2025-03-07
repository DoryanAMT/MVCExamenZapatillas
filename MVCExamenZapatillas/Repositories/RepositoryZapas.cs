using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MVCExamenZapatillas.Data;
using MVCExamenZapatillas.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace MVCExamenZapatillas.Repositories
{
    #region
    //create procedure SP_IMAGENES_ZAPATILLA_OUT
    //(@posicion int, @idproducto int, @regsitros int out)
    //as
    //select @regsitros = COUNT(IDIMAGEN)
    //from IMAGENESZAPASPRACTICA
    //where IDPRODUCTO = @idproducto
    //select IDIMAGEN, IMAGEN, IDPRODUCTO  from
    //(select CAST(ROW_NUMBER() over (order by IMAGEN) as int) as POSICION, IDIMAGEN, IDPRODUCTO,
    //IMAGEN from IMAGENESZAPASPRACTICA
    //where IDPRODUCTO = @idproducto) as query
    //where query.POSICION = @posicion
    //go

    //exec SP_IMAGENES_ZAPATILLA_OUT 2, 9, 0
    #endregion
    public class RepositoryZapas
    {
        private HospitalContext context;
        public RepositoryZapas(HospitalContext context)
        {
            this.context = context;
        }
        public async Task<List<Zapa>> GetZapasAsync()
        {
            return await this.context.Zapas.ToListAsync();
        }
        public async Task<Zapa> FindZapaAsync
            (int idZapa)
        {
            Zapa zapa = await this.context.Zapas
                .Where(x => x.IdProducto == idZapa)
                .FirstOrDefaultAsync();
            return zapa;
        }
        public async Task<ModelImagenesZapa> GetImagenesZapaOutAsync
            (int? posicion, int idZapa)
        {
            string sql = "SP_IMAGENES_ZAPATILLA_OUT @posicion, @idproducto, @registros out";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamIdProducto = new SqlParameter("@idproducto", idZapa);
            SqlParameter pamRegistros = new SqlParameter("@registros", 0);
            pamRegistros.Direction = System.Data.ParameterDirection.Output;

            var consulta =
                this.context.ImagenesZapas.FromSqlRaw(sql, pamPosicion, pamIdProducto, pamRegistros);
            List<ImagenesZapa> imagenesZapas = await consulta.ToListAsync();
            int registros = int.Parse(pamRegistros.Value.ToString());
            return new ModelImagenesZapa
            {
                NumeroRegistros = registros,
                Imagenes = imagenesZapas
            };
        }

    }
}
