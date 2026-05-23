# Fluxo Geral do Sistema BLH

## 1. Cadastros-base

### 1.1 Instituição BLH
- Nome do BLH
- CNES
- CNPJ
- endereço
- responsável técnico
- licença sanitária
- municípios atendidos
- postos de coleta vinculados
- hospitais/UTIs atendidos

Integrações:
- CNES/DATASUS para validar estabelecimento
- ViaCEP/IBGE para endereço, município e UF
- Receita Federal/CNPJ, se disponível

---

### 1.2 Usuários e perfis
Perfis:
- Administrador BLH
- Enfermeira/triagem
- Equipe de campo
- Laboratório/qualidade
- Estoque
- Distribuição
- Hospital/UTI
- Médico/Nutricionista
- Doadora

Todo acesso deve gerar log com:
- usuário
- data/hora
- ação realizada
- IP/dispositivo
- localização, quando mobile

---

### 1.3 Doadora
Cadastro completo da possível doadora:

- nome completo
- CPF
- CNS
- data nascimento
- telefone/WhatsApp
- endereço
- latitude/longitude
- dados do parto
- dados do bebê da doadora
- histórico clínico
- medicamentos
- tabagismo
- álcool/drogas
- tatuagem/piercing recente
- exames pré-natal
- termo de consentimento
- termo LGPD

Status:
- pré-cadastrada
- em triagem
- aguardando exames
- apta
- inapta
- suspensa
- inativa

O BLHWeb já prevê cadastro de doadoras, história pregressa, história atual, vínculo com rota e consulta doadora x receptor :contentReference[oaicite:0]{index=0}.

---

### 1.4 Receptor / Bebê
Cadastro do RN receptor:

- nome
- CNS, se houver
- prontuário hospitalar
- data nascimento
- idade gestacional
- peso
- diagnóstico
- hospital
- setor
- leito/incubadora
- responsável médico
- necessidade clínica
- prescrição médica/nutricional

Status:
- internado
- elegível para LHOP
- recebendo leite
- alta
- óbito
- bloqueado

A distribuição deve depender de prescrição ou solicitação médica/nutricional com volume, horário e necessidade do receptor :contentReference[oaicite:1]{index=1}.

---

## 2. Fluxo de captação da doadora

1. Doadora se cadastra pelo app ou é cadastrada pela equipe.
2. Sistema valida dados básicos.
3. Equipe realiza triagem clínica.
4. São anexados ou solicitados exames.
5. Enfermeira emite parecer:
   - apta
   - inapta
   - apta com restrição
   - pendente de documentação
6. Se aprovada, o sistema gera matrícula da doadora.
7. A doadora é vinculada a uma rota de coleta.

O sistema precisa ter o módulo “Parecer da Doadora”, como o BLHWeb já possui, com inclusão e consulta de pareceres :contentReference[oaicite:2]{index=2}.

---

## 3. Fluxo de rotas e coleta domiciliar

### Antes da coleta
O BLH monta a rota:

- dia da semana
- veículo
- motorista/profissional
- doadoras da rota
- quantidade prevista de frascos
- kits a entregar
- exames a coletar
- previsão de distância

O BLHWeb já possui cadastro de veículos, municípios, rotas, doadoras por rota, pré-rota, rota e relatório de coleta :contentReference[oaicite:3]{index=3}.

### Durante a coleta
No app da equipe de campo, registrar:

- check-in na residência
- latitude/longitude
- data/hora chegada
- data/hora saída
- temperatura da caixa térmica
- temperatura do leite/frascos
- quantidade de frascos coletados
- frascos estéreis entregues
- toucas/máscaras entregues
- coleta de exames, se houver
- assinatura da doadora
- fotos opcionais da caixa/frascos
- observações

Cada frasco coletado recebe ou confirma um **ID único** via QR Code/código de barras.

---

## 4. Fluxo do frasco

Cada frasco deve nascer no sistema com:

- ID único
- QR Code
- doadora
- data/hora da primeira ordenha
- data/hora da última ordenha
- volume
- tipo de leite:
  - colostro
  - transição
  - maduro
- origem:
  - coleta domiciliar
  - entrega no BLH
  - posto de coleta
- temperatura na coleta
- caixa térmica
- rota
- profissional responsável

O BLHWeb já prevê numeração e etiquetas de frascos crus, com alerta de cuidado para não quebrar a sequência de numeração :contentReference[oaicite:4]{index=4}.

---

## 5. Recepção no BLH

Ao chegar no BLH:

1. Profissional lê o QR Code da caixa térmica.
2. Sistema lista os frascos esperados da rota.
3. Cada frasco é conferido.
4. Registra:
   - temperatura de chegada
   - integridade da embalagem
   - rotulagem
   - volume
   - sujidade
   - tempo de transporte
   - conformidade
5. Se conforme: entra no estoque de leite cru.
6. Se não conforme: vai para perda/descarte com motivo.

O BLHWeb separa recepção por controle de visitas, recepção BLH, recepção por rota e consulta de frascos por data ou número :contentReference[oaicite:5]{index=5}.

---

## 6. Processamento do leite

Fluxo:

1. Seleção e classificação
2. Controle físico-químico
3. Degelo
4. Reenvase
5. Pasteurização
6. Controle microbiológico
7. Liberação ou descarte
8. Entrada no estoque pasteurizado

O BLHWeb contempla seleção e classificação, controle físico-químico, reenvase, pasteurização e controle de qualidade :contentReference[oaicite:6]{index=6}.

Cada novo frasco reenvasado precisa guardar vínculo com os frascos originais.

Exemplo:

```text
Frasco cru A + Frasco cru B
        ↓
Lote de processamento 001
        ↓
Frasco pasteurizado P001
````

Nunca pode perder a origem.

---

## 7. Controle de qualidade

Registrar:

* acidez Dornic
* crematócrito
* análise sensorial
* sujidade
* odor
* cor
* embalagem
* análise microbiológica
* resultado
* responsável técnico
* data/hora
* laudo

Status do frasco/lote:

* aguardando análise
* aprovado
* reprovado
* bloqueado
* descartado

A apresentação do BLHWeb reforça que o controle deve ser dinâmico, cobrindo todas as etapas, desde coleta até distribuição, com indicadores sensoriais, físico-químicos e microbiológicos .

---

## 8. Estoque

Separar:

### Estoque de leite cru

* freezer
* gaveta/prateleira
* temperatura
* validade
* doadora
* rota
* status

### Estoque de leite pasteurizado

* freezer
* lote
* frasco
* validade
* tipo de leite
* volume
* qualidade aprovada
* reservado para receptor
* disponível para distribuição

O BLHWeb já diferencia estoque de leite cru e leite pasteurizado, com consulta por frasco, freezer e doadora .

---

## 9. Solicitação hospitalar

O hospital acessa o portal e informa:

* hospital
* setor
* RN
* leito
* prescrição
* volume diário
* horários
* tipo de leite necessário
* justificativa clínica
* médico/nutricionista solicitante

O sistema calcula:

* estoque disponível
* prioridade do bebê
* tipo de leite compatível
* validade
* volume necessário

Prioridades:

* prematuro
* baixo peso
* infectado
* nutrição trófica
* imunodeficiência
* alergia a proteínas heterólogas
* casos excepcionais médicos .

---

## 10. Matching frasco → bebê

Esse é o coração do sistema.

O sistema só libera o frasco se:

* frasco aprovado no controle de qualidade
* dentro da validade
* temperatura sem ocorrência crítica
* prescrição válida
* bebê cadastrado
* hospital autorizado
* volume compatível
* status disponível

Ao separar o leite:

```text
Solicitação Hospitalar
       ↓
Prescrição do RN
       ↓
Matching automático
       ↓
Reserva de frascos
       ↓
Separação
       ↓
Conferência por QR Code
       ↓
Saída para hospital
```

Na saída, o sistema grava:

* ID do frasco
* ID do bebê
* ID da prescrição
* hospital
* setor
* responsável pela separação
* responsável pela entrega
* temperatura de saída
* caixa térmica
* data/hora

---

## 11. Distribuição

O BLHWeb já prevê distribuição de leite cru e pasteurizado para receptor, instituição BLH e posto de coleta, além de relatórios de distribuição .

No novo sistema, a distribuição deve ter:

* romaneio digital
* QR Code da remessa
* QR Code da caixa
* QR Code dos frascos
* temperatura de saída
* temperatura no trajeto
* GPS do transporte
* assinatura de entrega
* assinatura de recebimento

Status da remessa:

* em separação
* pronta para envio
* em transporte
* entregue
* recebida com conformidade
* recebida com não conformidade
* devolvida
* descartada

---

## 12. Recebimento no hospital

O hospital lê o QR Code da remessa e dos frascos.

Registra:

* data/hora recebimento
* temperatura chegada
* responsável pelo recebimento
* setor
* conferência dos frascos
* divergência, se houver

Se temperatura estiver fora do aceitável:

* frasco bloqueado
* BLH notificado
* auditoria aberta

---

## 13. Administração ao bebê

No momento de uso:

1. Profissional abre prescrição do bebê.
2. Escaneia pulseira/prontuário do bebê.
3. Escaneia QR Code do frasco.
4. Sistema valida se aquele frasco está reservado para aquele bebê.
5. Se correto, permite administração.
6. Registra:

   * volume administrado
   * data/hora
   * profissional
   * observações
   * sobra/descarte

Se o frasco não for do bebê:

```text
BLOQUEAR ADMINISTRAÇÃO
ALERTA CRÍTICO
REGISTRAR TENTATIVA
EXIGIR JUSTIFICATIVA
```

---

## 14. Consulta de rastreabilidade

O sistema deve permitir três consultas principais.

### Por frasco

Mostra:

* doadora
* coleta
* rota
* temperaturas
* processamento
* lote
* qualidade
* estoque
* distribuição
* hospital
* bebê receptor
* administração

### Por bebê

Mostra:

* todos os frascos recebidos
* volumes
* horários
* prescrições
* lote de cada frasco
* doadora de origem

### Por doadora

Mostra:

* frascos doados
* lotes gerados
* hospitais atendidos
* bebês receptores, com acesso restrito e anonimização quando necessário

---

## 15. Integrações com APIs públicas

### Governo / saúde

* CNES/DATASUS: validar hospitais, BLHs, postos e estabelecimentos.
* CNS/CadSUS: validar Cartão Nacional de Saúde de doadoras e receptores.
* IBGE Localidades: municípios, UF, códigos oficiais.
* ViaCEP: preenchimento de endereço.
* Receita Federal/CNPJ: validação de instituições, quando disponível.
* e-SUS/PEC ou sistemas municipais: futura integração com prontuário.
* RNDS/FHIR: integração futura para interoperabilidade em saúde.

O próprio BLHWeb já prevê consonância com Cartão Nacional de Saúde para doadores, receptores, profissionais e estabelecimentos .

---

## 16. Módulos finais do sistema

* Administração
* Cadastros
* Doadoras
* Receptores
* Triagem
* Exames
* Rotas
* Coleta domiciliar
* Recepção
* Frascos
* Produto
* Controle físico-químico
* Pasteurização
* Controle microbiológico
* Estoque
* Distribuição
* Hospital
* Administração ao RN
* Perdas/descarte
* Relatórios
* Auditoria
* Integrações públicas
* Dashboard de indicadores

---

## 17. Regra principal do sistema

Nenhum frasco pode chegar ao bebê sem que o sistema saiba:

```text
quem doou,
quando foi coletado,
onde foi coletado,
em qual temperatura foi transportado,
quem recebeu,
como foi processado,
qual lote gerou,
qual controle de qualidade aprovou,
onde ficou estocado,
quem separou,
quem transportou,
qual hospital recebeu,
qual bebê recebeu,
qual profissional administrou.
```

Essa é a solução completa para o problema: **controle absoluto do frasco até o bebê**.
