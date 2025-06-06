﻿namespace Plataforma.Educacao.Core.SharedDto.Conteudo;
public class AulaDto
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public string Descricao { get; set; }
    public short CargaHoraria { get; set; }
    public byte OrdemAula { get; set; }
    public bool Ativo { get; set; }
    public string Url { get; set; }
}
