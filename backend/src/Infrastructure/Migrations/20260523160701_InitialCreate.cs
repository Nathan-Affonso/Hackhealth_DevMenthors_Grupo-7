using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:perfil_usuario", "admin,blh_admin,enfermeira,campo,laboratorio,estoque,distribuicao,hospital,medico,nutricionista");

            migrationBuilder.CreateTable(
                name: "AdministracoesFrasco",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FrascoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceptorId = table.Column<Guid>(type: "uuid", nullable: false),
                    PrescricaoLeiteId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfissionalId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VolumeAdministradoMl = table.Column<decimal>(type: "numeric", nullable: false),
                    Observacoes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministracoesFrasco", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auditorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Acao = table.Column<string>(type: "text", nullable: false),
                    Entidade = table.Column<string>(type: "text", nullable: false),
                    EntidadeId = table.Column<Guid>(type: "uuid", nullable: false),
                    DadosAntes = table.Column<string>(type: "text", nullable: true),
                    DadosDepois = table.Column<string>(type: "text", nullable: true),
                    Ip = table.Column<string>(type: "text", nullable: true),
                    Dispositivo = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaixasTermicas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    CapacidadeLitros = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaixasTermicas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coletas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DoadoraId = table.Column<Guid>(type: "uuid", nullable: false),
                    RotaColetaId = table.Column<Guid>(type: "uuid", nullable: false),
                    CaixaTermicaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataHoraInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataHoraFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: true),
                    TemperaturaCaixaInicial = table.Column<decimal>(type: "numeric", nullable: true),
                    TemperaturaCaixaFinal = table.Column<decimal>(type: "numeric", nullable: true),
                    TemperaturaAmbiente = table.Column<decimal>(type: "numeric", nullable: true),
                    Observacoes = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coletas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ControlesQualidade",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FrascoId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    AcidezDornic = table.Column<decimal>(type: "numeric", nullable: true),
                    Crematocrito = table.Column<decimal>(type: "numeric", nullable: true),
                    CorConforme = table.Column<bool>(type: "boolean", nullable: false),
                    OdorConforme = table.Column<bool>(type: "boolean", nullable: false),
                    SujidadeAusente = table.Column<bool>(type: "boolean", nullable: false),
                    ResultadoMicrobiologico = table.Column<string>(type: "text", nullable: true),
                    Aprovado = table.Column<bool>(type: "boolean", nullable: false),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Observacoes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlesQualidade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DistribuicaoItens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DistribuicaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    FrascoId = table.Column<Guid>(type: "uuid", nullable: false),
                    VolumeMl = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistribuicaoItens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Distribuicoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PrescricaoLeiteId = table.Column<Guid>(type: "uuid", nullable: false),
                    HospitalId = table.Column<Guid>(type: "uuid", nullable: false),
                    CaixaTermicaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponsavelSeparacaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponsavelEntregaId = table.Column<Guid>(type: "uuid", nullable: true),
                    DataHoraSaida = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TemperaturaSaida = table.Column<decimal>(type: "numeric", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distribuicoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doadoras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    CPF = table.Column<string>(type: "text", nullable: false),
                    CNS = table.Column<string>(type: "text", nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Telefone = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Endereco = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: true),
                    DataParto = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TipoParto = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doadoras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estoques",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FrascoId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoEstoque = table.Column<int>(type: "integer", nullable: false),
                    Freezer = table.Column<string>(type: "text", nullable: false),
                    Prateleira = table.Column<string>(type: "text", nullable: true),
                    Posicao = table.Column<string>(type: "text", nullable: true),
                    TemperaturaAtual = table.Column<decimal>(type: "numeric", nullable: true),
                    DataEntrada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataValidade = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estoques", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExamesDoadora",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DoadoraId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoExame = table.Column<string>(type: "text", nullable: false),
                    DataExame = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Resultado = table.Column<string>(type: "text", nullable: false),
                    ArquivoUrl = table.Column<string>(type: "text", nullable: true),
                    Valido = table.Column<bool>(type: "boolean", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamesDoadora", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Frascos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoUnico = table.Column<string>(type: "text", nullable: false),
                    QrCode = table.Column<string>(type: "text", nullable: true),
                    DoadoraId = table.Column<Guid>(type: "uuid", nullable: false),
                    ColetaId = table.Column<Guid>(type: "uuid", nullable: false),
                    CaixaTermicaId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoteId = table.Column<Guid>(type: "uuid", nullable: true),
                    TipoLeite = table.Column<int>(type: "integer", nullable: false),
                    VolumeMl = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataOrdenhaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataOrdenhaFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TemperaturaColeta = table.Column<decimal>(type: "numeric", nullable: true),
                    TemperaturaRecepcao = table.Column<decimal>(type: "numeric", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frascos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instituicoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    CNES = table.Column<string>(type: "text", nullable: true),
                    CNPJ = table.Column<string>(type: "text", nullable: true),
                    Endereco = table.Column<string>(type: "text", nullable: false),
                    Municipio = table.Column<string>(type: "text", nullable: false),
                    UF = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Ativa = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instituicoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LotesProcessamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoLote = table.Column<string>(type: "text", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Observacoes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotesProcessamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrescricoesLeite",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceptorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfissionalSolicitanteId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoLeiteSolicitado = table.Column<string>(type: "text", nullable: false),
                    VolumeDiarioMl = table.Column<decimal>(type: "numeric", nullable: false),
                    VolumePorHorarioMl = table.Column<decimal>(type: "numeric", nullable: false),
                    Horarios = table.Column<string>(type: "text", nullable: false),
                    JustificativaClinica = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescricoesLeite", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessamentosFrasco",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    FrascoOrigemId = table.Column<Guid>(type: "uuid", nullable: false),
                    FrascoDestinoId = table.Column<Guid>(type: "uuid", nullable: true),
                    TipoProcessamento = table.Column<int>(type: "integer", nullable: false),
                    DataHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: false),
                    Observacoes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessamentosFrasco", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecebimentosHospital",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DistribuicaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    HospitalId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponsavelRecebimentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TemperaturaChegada = table.Column<decimal>(type: "numeric", nullable: false),
                    Conforme = table.Column<bool>(type: "boolean", nullable: false),
                    Observacoes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecebimentosHospital", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecepcoesBLH",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FrascoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TemperaturaChegada = table.Column<decimal>(type: "numeric", nullable: false),
                    EmbalagemIntegra = table.Column<bool>(type: "boolean", nullable: false),
                    RotuloConforme = table.Column<bool>(type: "boolean", nullable: false),
                    VolumeConforme = table.Column<bool>(type: "boolean", nullable: false),
                    Conforme = table.Column<bool>(type: "boolean", nullable: false),
                    MotivoNaoConformidade = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecepcoesBLH", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Receptores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    CNS = table.Column<string>(type: "text", nullable: true),
                    Prontuario = table.Column<string>(type: "text", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdadeGestacional = table.Column<int>(type: "integer", nullable: true),
                    PesoNascimento = table.Column<decimal>(type: "numeric", nullable: true),
                    Diagnostico = table.Column<string>(type: "text", nullable: true),
                    HospitalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Setor = table.Column<string>(type: "text", nullable: true),
                    Leito = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receptores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosRastreamentoCaixa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CaixaTermicaId = table.Column<Guid>(type: "uuid", nullable: false),
                    RotaColetaId = table.Column<Guid>(type: "uuid", nullable: true),
                    ColetaId = table.Column<Guid>(type: "uuid", nullable: true),
                    DistribuicaoId = table.Column<Guid>(type: "uuid", nullable: true),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoRegistro = table.Column<int>(type: "integer", nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Temperatura = table.Column<decimal>(type: "numeric", nullable: false),
                    DataHoraRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Observacao = table.Column<string>(type: "text", nullable: true),
                    OrigemRegistro = table.Column<int>(type: "integer", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosRastreamentoCaixa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RotasColeta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    DiaSemana = table.Column<string>(type: "text", nullable: true),
                    Veiculo = table.Column<string>(type: "text", nullable: true),
                    Motorista = table.Column<string>(type: "text", nullable: true),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RotasColeta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TriagensDoadora",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DoadoraId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemHIV = table.Column<bool>(type: "boolean", nullable: false),
                    TemHepatite = table.Column<bool>(type: "boolean", nullable: false),
                    TemSifilis = table.Column<bool>(type: "boolean", nullable: false),
                    UsaMedicamento = table.Column<bool>(type: "boolean", nullable: false),
                    Medicamentos = table.Column<string>(type: "text", nullable: true),
                    Tabagista = table.Column<bool>(type: "boolean", nullable: false),
                    UsaAlcool = table.Column<bool>(type: "boolean", nullable: false),
                    UsaDrogas = table.Column<bool>(type: "boolean", nullable: false),
                    TatuagemOuPiercingUltimos12Meses = table.Column<bool>(type: "boolean", nullable: false),
                    Aprovada = table.Column<bool>(type: "boolean", nullable: false),
                    Parecer = table.Column<string>(type: "text", nullable: true),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriagensDoadora", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    SenhaHash = table.Column<string>(type: "text", nullable: false),
                    Perfil = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CaixasTermicas",
                columns: new[] { "Id", "CapacidadeLitros", "Codigo", "Descricao", "Status" },
                values: new object[] { new Guid("66666666-6666-6666-6666-666666666666"), 12m, "CX-001", "Caixa termica principal", 1 });

            migrationBuilder.InsertData(
                table: "Coletas",
                columns: new[] { "Id", "CaixaTermicaId", "DataHoraFim", "DataHoraInicio", "DoadoraId", "Latitude", "Longitude", "Observacoes", "ResponsavelId", "RotaColetaId", "Status", "TemperaturaAmbiente", "TemperaturaCaixaFinal", "TemperaturaCaixaInicial" },
                values: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 5, 23, 10, 35, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 23, 10, 20, 0, 0, DateTimeKind.Utc), new Guid("44444444-4444-4444-4444-444444444444"), -22.2088m, -49.9345m, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("55555555-5555-5555-5555-555555555555"), 2, 24m, 4.2m, 3.8m });

            migrationBuilder.InsertData(
                table: "ControlesQualidade",
                columns: new[] { "Id", "AcidezDornic", "Aprovado", "CorConforme", "Crematocrito", "DataHora", "FrascoId", "LoteId", "Observacoes", "OdorConforme", "ResponsavelId", "ResultadoMicrobiologico", "SujidadeAusente" },
                values: new object[] { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), 4m, true, true, 6m, new DateTime(2026, 5, 23, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("88888888-8888-8888-8888-888888888888"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, true, new Guid("11111111-1111-1111-1111-111111111111"), "Ausente", true });

            migrationBuilder.InsertData(
                table: "Doadoras",
                columns: new[] { "Id", "CNS", "CPF", "CriadoEm", "DataNascimento", "DataParto", "Email", "Endereco", "Latitude", "Longitude", "Nome", "Status", "Telefone", "TipoParto" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), "700000000000001", "12345678901", new DateTime(2026, 5, 23, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(1995, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "maria@example.com", "Rua das Flores, 50", -22.2088m, -49.9345m, "Maria Doadora", 3, "(14) 99999-0001", "Normal" });

            migrationBuilder.InsertData(
                table: "Estoques",
                columns: new[] { "Id", "DataEntrada", "DataValidade", "FrascoId", "Freezer", "Posicao", "Prateleira", "Status", "TemperaturaAtual", "TipoEstoque" },
                values: new object[] { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new DateTime(2026, 5, 23, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 23, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("88888888-8888-8888-8888-888888888888"), "FZ-01", "01", "A", 1, -18m, 1 });

            migrationBuilder.InsertData(
                table: "Frascos",
                columns: new[] { "Id", "CaixaTermicaId", "CodigoUnico", "ColetaId", "CriadoEm", "DataOrdenhaFim", "DataOrdenhaInicio", "DoadoraId", "LoteId", "QrCode", "Status", "TemperaturaColeta", "TemperaturaRecepcao", "TipoLeite", "VolumeMl" },
                values: new object[] { new Guid("88888888-8888-8888-8888-888888888888"), new Guid("66666666-6666-6666-6666-666666666666"), "BLH-2026-0001", new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 5, 23, 12, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "BLH-2026-0001", 6, 4.1m, 4.4m, 2, 120m });

            migrationBuilder.InsertData(
                table: "Instituicoes",
                columns: new[] { "Id", "Ativa", "CNES", "CNPJ", "Endereco", "Latitude", "Longitude", "Municipio", "Nome", "Tipo", "UF" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222222"), true, "0000001", "00000000000100", "Av. Central, 100", -22.2135m, -49.9450m, "Marilia", "Banco de Leite Humano Exemplo", 0, "SP" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), true, "0000002", "00000000000200", "Rua Saude, 200", -22.2170m, -49.9505m, "Marilia", "Hospital Materno Infantil", 1, "SP" }
                });

            migrationBuilder.InsertData(
                table: "LotesProcessamento",
                columns: new[] { "Id", "CodigoLote", "DataFim", "DataInicio", "Observacoes", "ResponsavelId", "Status" },
                values: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "LOTE-2026-001", new DateTime(2026, 5, 23, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 22, 12, 0, 0, 0, DateTimeKind.Utc), "Lote seed aprovado", new Guid("11111111-1111-1111-1111-111111111111"), 3 });

            migrationBuilder.InsertData(
                table: "PrescricoesLeite",
                columns: new[] { "Id", "CriadoEm", "Horarios", "JustificativaClinica", "ProfissionalSolicitanteId", "ReceptorId", "Status", "TipoLeiteSolicitado", "VolumeDiarioMl", "VolumePorHorarioMl" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 5, 23, 12, 0, 0, 0, DateTimeKind.Utc), "06:00,09:00,12:00,15:00,18:00,21:00", "Prematuridade e baixo peso", new Guid("11111111-1111-1111-1111-111111111111"), new Guid("99999999-9999-9999-9999-999999999999"), 0, "Pasteurizado", 120m, 20m });

            migrationBuilder.InsertData(
                table: "Receptores",
                columns: new[] { "Id", "CNS", "DataNascimento", "Diagnostico", "HospitalId", "IdadeGestacional", "Leito", "Nome", "PesoNascimento", "Prontuario", "Setor", "Status" },
                values: new object[] { new Guid("99999999-9999-9999-9999-999999999999"), "700000000000002", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Prematuridade", new Guid("33333333-3333-3333-3333-333333333333"), 31, "03", "Bebe Receptor", 1.25m, "PR-001", "UTI Neonatal", 1 });

            migrationBuilder.InsertData(
                table: "RegistrosRastreamentoCaixa",
                columns: new[] { "Id", "CaixaTermicaId", "ColetaId", "CriadoEm", "DataHoraRegistro", "DistribuicaoId", "Latitude", "Longitude", "Observacao", "OrigemRegistro", "RotaColetaId", "Temperatura", "TipoRegistro", "UsuarioId" },
                values: new object[,]
                {
                    { new Guid("eeeeeeee-0001-0000-0000-000000000001"), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 5, 23, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 23, 10, 0, 0, 0, DateTimeKind.Utc), null, -22.2135m, -49.9450m, "Saida do BLH", 1, new Guid("55555555-5555-5555-5555-555555555555"), 3.7m, 0, new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("eeeeeeee-0002-0000-0000-000000000002"), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 5, 23, 10, 8, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 23, 10, 8, 0, 0, DateTimeKind.Utc), null, -22.2116m, -49.9418m, "Temperatura durante deslocamento", 1, new Guid("55555555-5555-5555-5555-555555555555"), 3.9m, 3, new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("eeeeeeee-0003-0000-0000-000000000003"), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 5, 23, 10, 20, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 23, 10, 20, 0, 0, DateTimeKind.Utc), null, -22.2088m, -49.9345m, "Chegada na doadora", 1, new Guid("55555555-5555-5555-5555-555555555555"), 4.1m, 1, new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("eeeeeeee-0004-0000-0000-000000000004"), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 5, 23, 10, 35, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 23, 10, 35, 0, 0, DateTimeKind.Utc), null, -22.2088m, -49.9345m, "Coleta realizada", 1, new Guid("55555555-5555-5555-5555-555555555555"), 4.2m, 2, new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("eeeeeeee-0005-0000-0000-000000000005"), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 5, 23, 10, 55, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 23, 10, 55, 0, 0, DateTimeKind.Utc), null, -22.2135m, -49.9450m, "Retorno ao BLH", 1, new Guid("55555555-5555-5555-5555-555555555555"), 4.4m, 4, new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.InsertData(
                table: "RotasColeta",
                columns: new[] { "Id", "DataFim", "DataInicio", "DiaSemana", "Motorista", "Nome", "ResponsavelId", "Status", "Veiculo" },
                values: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), null, new DateTime(2026, 5, 23, 10, 0, 0, 0, DateTimeKind.Utc), "Segunda", "Joao", "Rota Norte - Manha", new Guid("11111111-1111-1111-1111-111111111111"), 1, "Van BLH 01" });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Ativo", "CriadoEm", "Email", "Nome", "Perfil", "SenhaHash" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), true, new DateTime(2026, 5, 23, 10, 0, 0, 0, DateTimeKind.Utc), "admin@blh.local", "Administrador BLH", 0, "seed-admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Frascos_CodigoUnico",
                table: "Frascos",
                column: "CodigoUnico",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosRastreamentoCaixa_CaixaTermicaId_DataHoraRegistro",
                table: "RegistrosRastreamentoCaixa",
                columns: new[] { "CaixaTermicaId", "DataHoraRegistro" });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdministracoesFrasco");

            migrationBuilder.DropTable(
                name: "Auditorias");

            migrationBuilder.DropTable(
                name: "CaixasTermicas");

            migrationBuilder.DropTable(
                name: "Coletas");

            migrationBuilder.DropTable(
                name: "ControlesQualidade");

            migrationBuilder.DropTable(
                name: "DistribuicaoItens");

            migrationBuilder.DropTable(
                name: "Distribuicoes");

            migrationBuilder.DropTable(
                name: "Doadoras");

            migrationBuilder.DropTable(
                name: "Estoques");

            migrationBuilder.DropTable(
                name: "ExamesDoadora");

            migrationBuilder.DropTable(
                name: "Frascos");

            migrationBuilder.DropTable(
                name: "Instituicoes");

            migrationBuilder.DropTable(
                name: "LotesProcessamento");

            migrationBuilder.DropTable(
                name: "PrescricoesLeite");

            migrationBuilder.DropTable(
                name: "ProcessamentosFrasco");

            migrationBuilder.DropTable(
                name: "RecebimentosHospital");

            migrationBuilder.DropTable(
                name: "RecepcoesBLH");

            migrationBuilder.DropTable(
                name: "Receptores");

            migrationBuilder.DropTable(
                name: "RegistrosRastreamentoCaixa");

            migrationBuilder.DropTable(
                name: "RotasColeta");

            migrationBuilder.DropTable(
                name: "TriagensDoadora");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
