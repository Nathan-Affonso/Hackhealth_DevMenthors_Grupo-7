# Documentação Funcional — Sistema de Banco de Leite Humano (BLH)

Com base:

* no manual oficial da Anvisa/Rede BLH-BR 
* no protocolo operacional do BLH de Bauru 
* e nos fluxos apresentados nas imagens anexadas,

o sistema precisa ser projetado como uma plataforma de rastreabilidade ponta a ponta do leite humano, desde a captação da possível doadora até a administração do frasco ao recém-nascido na UTI neonatal.

---

# 1. Objetivo do Sistema

O sistema deve garantir:

* rastreabilidade completa do leite humano;
* segurança sanitária;
* conformidade com RDC 171/2006;
* controle logístico;
* auditoria total;
* controle de temperatura e cadeia fria;
* vínculo entre:

  * doadora,
  * coleta,
  * frasco,
  * processamento,
  * pasteurização,
  * distribuição,
  * bebê receptor.

O principal objetivo crítico é:

> Saber exatamente QUAL FRASCO foi administrado para QUAL BEBÊ, em QUAL DATA, por QUAL PROFISSIONAL, em QUAL HOSPITAL.

---

# 2. Arquitetura Geral da Plataforma

# 2.1 Sistemas envolvidos

## 1. Sistema Web Administrativo (Sede BLH)

Responsável por:

* cadastro geral;
* triagem;
* exames;
* processamento;
* estoque;
* pasteurização;
* rastreabilidade;
* distribuição;
* auditoria;
* relatórios sanitários;
* dashboards;
* conformidade.

---

## 2. Aplicativo Mobile da Doadora

Responsável por:

* cadastro inicial;
* orientações;
* solicitação de coleta;
* registro de ordenha;
* acompanhamento de coleta;
* notificações;
* registro de armazenamento doméstico;
* assinatura de termos;
* geolocalização opcional da coleta.

---

## 3. Aplicativo Mobile da Equipe de Campo

Responsável por:

* rotas;
* coleta domiciliar;
* GPS;
* temperatura;
* checklist sanitário;
* coleta de exames;
* registro fotográfico;
* entrega de frascos;
* leitura QR Code;
* cadeia fria;
* transporte.

---

## 4. Sistema Hospitalar / UTI Neonatal

Responsável por:

* recebimento de frascos;
* entrada em estoque hospitalar;
* prescrição;
* vinculação frasco → RN;
* administração;
* rastreabilidade;
* devoluções;
* descarte.

---

# 3. Macrofluxo Operacional

---

# ETAPA 1 — Captação da Doadora

Conforme apresentado no fluxo “Fase 1: Captação e Triagem Inicial”.

## Objetivo

Identificar possíveis doadoras aptas.

---

## Dados obrigatórios

## Identificação

* nome completo;
* CPF;
* CNS;
* data nascimento;
* telefone;
* WhatsApp;
* e-mail;
* endereço completo;
* latitude/longitude da residência;
* ponto de referência.

---

## Dados obstétricos

* data do parto;
* idade gestacional;
* tipo de parto;
* peso do RN;
* complicações gestacionais.

---

## Dados clínicos

* hipertensão;
* diabetes;
* hepatite;
* HIV;
* sífilis;
* uso de medicamentos;
* transfusão;
* tabagismo;
* álcool;
* drogas;
* tatuagem recente;
* piercing recente.

Conforme protocolo BLH Bauru 

---

## Exames obrigatórios

* Hemograma
* Glicemia
* HIV I e II
* Sífilis
* Hepatite B
* Hepatite C
* Toxoplasmose IgG/IgM

---

## Funcionalidades

* upload de exames;
* assinatura digital;
* termo LGPD;
* termo de doação;
* workflow de aprovação;
* status:

  * pré-cadastro;
  * em triagem;
  * apta;
  * inapta;
  * suspensa.

---

# 4. ETAPA 2 — Avaliação da Cadeia Fria Residencial

Conforme fluxo:
“O Funil de Segurança da Doadora”.

---

## Verificações obrigatórias

* possui freezer?;
* temperatura média;
* possui refrigerador duplex?;
* leite armazenado junto a outros alimentos?;
* frequência de queda de energia;
* higiene doméstica;
* utensílios adequados.

---

## Funcionalidades

### Mobile da equipe

* checklist;
* fotos;
* GPS;
* assinatura;
* temperatura do freezer;
* leitura bluetooth de termômetro.

---

# 5. ETAPA 3 — Cadastro da Doadora

Após aprovação:

## Sistema gera:

* matrícula da doadora;
* QR Code;
* etiqueta;
* número interno;
* vínculo de coleta.

---

## Status possíveis

* ativa;
* suspensa;
* bloqueada;
* finalizada.

---

# 6. ETAPA 4 — Controle de Frascos

Um dos pontos mais críticos do sistema.

---

# Cada frasco precisa possuir:

* ID único global;
* QR Code;
* código de barras;
* RFID opcional;
* vínculo com:

  * doadora;
  * coleta;
  * lote;
  * pasteurização.

---

# Informações do frasco

## Origem

* doadora;
* data coleta;
* hora coleta;
* tipo leite:

  * colostro,
  * transição,
  * maduro.

---

## Cadeia fria

* temperatura coleta;
* temperatura transporte;
* temperatura recepção;
* temperatura processamento.

---

## Situação

* cru;
* congelado;
* em análise;
* descartado;
* pasteurizado;
* distribuído;
* administrado;
* devolvido.

---

# 7. ETAPA 5 — Aplicativo da Equipe de Campo

Conforme fluxo:
“Matriz Logística: Coleta Domiciliar”.

---

# Funcionalidades principais

## Rotas

* mapa;
* otimização;
* agenda semanal;
* regiões.

---

## Durante coleta

Registrar:

* horário;
* GPS;
* temperatura da caixa térmica;
* temperatura ambiente;
* quantidade frascos;
* fotos;
* assinatura;
* coleta de sangue;
* entrega de kits esterilizados.

---

## Segurança

* rastreamento em tempo real;
* histórico de trajeto;
* alerta de desvio;
* modo offline.

---

# 8. ETAPA 6 — Recepção no BLH

Conforme slide:
“Recepção no BLH e Inspeção de Conformidade”.

---

# Checklist obrigatório

## Integridade

* frasco íntegro;
* tampa correta;
* vedação;
* validade.

---

## Cadeia fria

* temperatura adequada;
* tempo transporte;
* gelo suficiente.

---

## Volume

* volume total;
* volume individual.

---

## Resultado

* conforme;
* não conforme;
* descarte.

---

# Registro obrigatório

Conforme manual da Anvisa:
o sistema deve manter rastreabilidade integral do produto 

---

# 9. ETAPA 7 — Processamento

Conforme fluxo:
“Pasteurização e Reenvase”.

---

# Processo

## 1. Degelo

Registrar:

* início;
* fim;
* operador;
* temperatura.

---

## 2. Seleção

Classificar:

* colostro;
* transição;
* maduro.

---

## 3. Reenvase

Novo frasco:

* novo ID;
* vínculo ao original;
* lote.

---

## 4. Pasteurização

Registrar:

* equipamento;
* operador;
* curva térmica;
* temperatura;
* tempo;
* lote.

---

# 10. ETAPA 8 — Controle de Qualidade

Conforme:
“Cadernos 1, 2 e 3”.

---

# Sistema deve armazenar

## Controle microbiológico

* cultura;
* resultado;
* validade;
* aprovação.

---

## Controle físico-químico

* crematócrito;
* densidade;
* acidez Dornic.

---

## Não conformidades

* contaminação;
* quebra cadeia fria;
* falha equipamento;
* descarte.

---

# 11. ETAPA 9 — Estoque Inteligente

Controle de:

* freezer;
* posição;
* temperatura;
* validade;
* FIFO/FEFO;
* lotes.

---

# Alertas automáticos

* temperatura alta;
* vencimento;
* queda energia;
* lote bloqueado.

---

# 12. ETAPA 10 — Solicitação Hospitalar

Conforme fluxo:
“Demanda Hospitalar e Preparação para Liberação”.

---

# Hospital informa

* bebê;
* prescrição;
* volume;
* horários;
* tipo leite;
* prioridade clínica.

---

# Critérios RDC

Conforme protocolo BLH Bauru 

Prioridade:

* prematuros;
* baixo peso;
* enteroinfecção;
* imunodeficiência;
* alergia alimentar.

---

# 13. ETAPA 11 — Distribuição

Ponto MAIS CRÍTICO do sistema.

---

# Cada envio precisa registrar

* hospital;
* setor;
* UTI;
* responsável entrega;
* responsável recebimento;
* horário;
* temperatura saída;
* temperatura chegada;
* GPS trajeto.

---

# 14. ETAPA 12 — Administração ao RN

Fluxo final da rastreabilidade.

---

# Sistema hospitalar precisa registrar

## Bebê

* nome;
* prontuário;
* incubadora;
* leito.

---

## Administração

* ID frasco;
* horário;
* volume administrado;
* profissional;
* observações.

---

# Resultado final

O sistema deve permitir:

## Consulta reversa

### Pelo bebê:

“Quais frascos ele recebeu?”

### Pelo frasco:

“Qual bebê recebeu?”

### Pela doadora:

“Quais RNs receberam leite dela?”

---

# 15. Rastreabilidade Completa (Obrigatória)

Conforme fluxo apresentado:
“Engenharia da Rastreabilidade”.

---

# Cadeia obrigatória

Doadora
→ coleta
→ transporte
→ recepção
→ processamento
→ lote
→ pasteurização
→ estoque
→ distribuição
→ hospital
→ bebê

---

# 16. Auditoria e Compliance

Sistema deve registrar:

* usuário;
* IP;
* GPS;
* dispositivo;
* data/hora;
* alterações;
* logs imutáveis.

---

# 17. Controle de Temperatura

Um dos módulos mais importantes.

---

# Pontos monitorados

* residência;
* coleta;
* caixa térmica;
* transporte;
* recepção;
* freezer;
* pasteurização;
* distribuição;
* hospital.

---

# Funcionalidades

* sensores IoT;
* bluetooth;
* alarmes;
* dashboards;
* gráficos históricos;
* eventos críticos.

---

# 18. Perfis de Usuário

## Administrativo BLH

* acesso total.

---

## Enfermeira

* triagem;
* doadora;
* coleta.

---

## Biomédico/Laboratório

* qualidade;
* exames.

---

## Equipe Campo

* coleta;
* transporte.

---

## Hospital

* recebimento;
* administração.

---

## Doadora

* app limitado.

---

# 19. Dashboards

## Operacional

* litros coletados;
* frascos;
* doadoras ativas;
* coletas dia.

---

## Qualidade

* descartes;
* contaminações;
* temperatura;
* falhas.

---

## Neonatal

* bebês atendidos;
* volume distribuído;
* hospitais.

---

# 20. Integrações Futuras

* prontuário eletrônico;
* HL7/FHIR;
* equipamentos IoT;
* sensores temperatura;
* WhatsApp;
* SMS;
* gov.br;
* cartão SUS.

---

# 21. Banco de Dados — Principais Entidades

## Núcleo principal

* Doadora
* Exame
* Coleta
* Frasco
* Lote
* Pasteurização
* ControleQualidade
* Estoque
* Distribuição
* Hospital
* RecémNascido
* Administração
* Temperatura
* LocalizaçãoGPS
* Usuário
* Auditoria

---

# 22. Regras Críticas do Negócio

## Regra 1

Frasco SEM rastreabilidade completa:
→ NÃO pode ser distribuído.

---

## Regra 2

Temperatura fora da faixa:
→ bloqueio automático.

---

## Regra 3

Doadora com exame vencido:
→ suspensão automática.

---

## Regra 4

Frasco administrado:
→ estoque encerrado.

---

## Regra 5

Qualquer alteração deve ser auditável.

---

# 23. Requisitos Legais e Normativos

Baseado em:

* RDC 171/2006
* Rede BLH-BR
* Manual Anvisa BLH 
* Protocolos municipais BLH 
* LGPD
* normas sanitárias estaduais.

---

# 24. Recomendação Técnica de Arquitetura

## Backend

* .NET 8
* PostgreSQL
* RabbitMQ
* Redis

---

## Mobile

* Flutter

---

## Web

* React + TypeScript

---

## Infra

* Docker
* Kubernetes
* observabilidade;
* logs centralizados.

---

# 25. Recomendação Mais Importante

O coração do sistema NÃO é o estoque.

O coração do sistema é:

# RASTREABILIDADE SANITÁRIA

O sistema inteiro deve ser desenhado para responder rapidamente:

> “Qual foi o caminho completo desse leite desde a doadora até o bebê?”
