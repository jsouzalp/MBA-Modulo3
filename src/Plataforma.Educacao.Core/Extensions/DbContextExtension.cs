using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Core.Extensions;
public static class DbContextExtensions
{
    public static void AtualizarEstadoValueObject<T>(this DbContext context, T antigo, T novo)
        where T : class
    {
        if (antigo != null)
            context.Entry(antigo).State = EntityState.Deleted;

        if (novo != null)
            context.Entry(novo).State = EntityState.Added;
    }
}