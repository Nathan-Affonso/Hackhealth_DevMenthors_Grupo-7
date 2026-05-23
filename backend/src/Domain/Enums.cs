namespace Domain;

public enum PerfilUsuario { Admin, BLH_Admin, Enfermeira, Campo, Laboratorio, Estoque, Distribuicao, Hospital, Medico, Nutricionista }
public enum TipoInstituicao { BLH, Hospital, UTI, PostoColeta }
public enum StatusDoadora { PreCadastro, EmTriagem, AguardandoExames, Apta, Inapta, Suspensa, Inativa }
public enum StatusRota { Planejada, EmAndamento, Finalizada, Cancelada }
public enum StatusCaixaTermica { Disponivel, EmRota, Higienizacao, Manutencao, Inativa }
public enum StatusColeta { Agendada, EmAndamento, Realizada, Cancelada }
public enum TipoLeite { Colostro, Transicao, Maduro, NaoClassificado }
public enum StatusFrasco { Coletado, Recebido, EmAnalise, CruEstocado, EmProcessamento, Pasteurizado, Aprovado, Reprovado, Distribuido, RecebidoHospital, Administrado, Descartado }
public enum TipoRegistroCaixa { SaidaBLH, ChegadaDoadora, ColetaRealizada, TemperaturaDuranteRota, RetornoBLH, SaidaHospital, ChegadaHospital, RetornoDistribuicao }
public enum OrigemRegistro { Web, Mobile, SensorIoT }
public enum StatusLote { Aberto, EmProcessamento, AguardandoQualidade, Aprovado, Reprovado }
public enum TipoProcessamento { Degelo, SelecaoClassificacao, Reenvase, Pasteurizacao }
public enum TipoEstoque { Cru, Pasteurizado }
public enum StatusEstoque { Disponivel, Reservado, Bloqueado, Saiu, Descartado }
public enum StatusReceptor { Internado, Elegivel, RecebendoLeite, Alta, Obito, Bloqueado }
public enum StatusPrescricao { Aberta, Atendida, Cancelada }
public enum StatusDistribuicao { EmSeparacao, EmTransporte, Entregue, Cancelada }
