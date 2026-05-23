import 'package:flutter/material.dart';

// ─────────────────────────────────────────
// THEME — BLH Rastreabilidade (verde escuro)
// ─────────────────────────────────────────
class AppTheme {
  static const Color primary = Color(0xFF1B5E37); // verde escuro do logo
  static const Color primaryDark = Color(0xFF134228);
  static const Color primaryLight = Color(0xFFE8F5EE);
  static const Color accent = Color(0xFF2E7D52);
  static const Color success = Color(0xFF22C55E);
  static const Color successLight = Color(0xFFDCFCE7);
  static const Color background = Color(0xFFF5F7F5);
  static const Color surface = Color(0xFFFFFFFF);
  static const Color textPrimary = Color(0xFF1A2E22);
  static const Color textSecondary = Color(0xFF5A7A65);
  static const Color textHint = Color(0xFF9CA3AF);
  static const Color border = Color(0xFFD4E4DA);
  static const Color gpsGreen = Color(0xFF16A34A);
  static const Color sidebarBg = Color(0xFF1B5E37);

  static ThemeData get theme => ThemeData(
        useMaterial3: true,
        scaffoldBackgroundColor: background,
        colorScheme: ColorScheme.fromSeed(
            seedColor: primary, primary: primary, background: background),
        appBarTheme: const AppBarTheme(
          backgroundColor: surface,
          foregroundColor: textPrimary,
          elevation: 0,
          centerTitle: false,
          titleTextStyle: TextStyle(
              color: textPrimary,
              fontSize: 17,
              fontWeight: FontWeight.w700,
              letterSpacing: -0.3),
        ),
        elevatedButtonTheme: ElevatedButtonThemeData(
            style: ElevatedButton.styleFrom(
          backgroundColor: primary,
          foregroundColor: Colors.white,
          minimumSize: const Size(double.infinity, 52),
          shape:
              RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
          textStyle: const TextStyle(fontSize: 15, fontWeight: FontWeight.w600),
          elevation: 0,
        )),
        outlinedButtonTheme: OutlinedButtonThemeData(
            style: OutlinedButton.styleFrom(
          foregroundColor: primary,
          minimumSize: const Size(double.infinity, 52),
          shape:
              RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
          side: const BorderSide(color: primary, width: 1.5),
          textStyle: const TextStyle(fontSize: 15, fontWeight: FontWeight.w600),
        )),
        inputDecorationTheme: InputDecorationTheme(
          filled: true,
          fillColor: surface,
          contentPadding:
              const EdgeInsets.symmetric(horizontal: 16, vertical: 14),
          border: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8),
              borderSide: const BorderSide(color: border)),
          enabledBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8),
              borderSide: const BorderSide(color: border)),
          focusedBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8),
              borderSide: const BorderSide(color: primary, width: 1.5)),
          hintStyle: const TextStyle(color: textHint, fontSize: 14),
        ),
      );
}

// ─────────────────────────────────────────
// BLH LOGO WIDGET — recria o logo SVG-like
// ─────────────────────────────────────────
class BLHLogo extends StatelessWidget {
  final double size;
  final bool showText;
  final Color color;
  const BLHLogo({
    super.key,
    this.size = 40,
    this.showText = true,
    this.color = const Color(0xFF1B5E37),
  });

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [
        // Ícone gota com mãe e bebê
        SizedBox(
          width: size,
          height: size,
          child: CustomPaint(painter: _BLHIconPainter(color: color)),
        ),
        if (showText) ...[
          const SizedBox(width: 10),
          Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'BLH',
                style: TextStyle(
                  fontSize: size * 0.55,
                  fontWeight: FontWeight.w900,
                  color: color,
                  letterSpacing: 1,
                  height: 1.0,
                ),
              ),
              Text(
                'RASTREABILIDADE',
                style: TextStyle(
                  fontSize: size * 0.18,
                  fontWeight: FontWeight.w600,
                  color: color,
                  letterSpacing: 1.5,
                  height: 1.2,
                ),
              ),
            ],
          ),
        ],
      ],
    );
  }
}

class _BLHIconPainter extends CustomPainter {
  final Color color;
  const _BLHIconPainter({required this.color});

  @override
  void paint(Canvas canvas, Size size) {
    final paint = Paint()
      ..color = color
      ..style = PaintingStyle.stroke
      ..strokeWidth = size.width * 0.06
      ..strokeCap = StrokeCap.round;

    final fillPaint = Paint()
      ..color = color
      ..style = PaintingStyle.fill;

    final cx = size.width / 2;
    final cy = size.height / 2;
    final r = size.width * 0.42;

    // Gota (drop shape)
    final dropPath = Path();
    dropPath.moveTo(cx, size.height * 0.05);
    dropPath.cubicTo(
      cx + r * 1.1,
      cy * 0.5,
      cx + r,
      size.height * 0.75,
      cx,
      size.height * 0.97,
    );
    dropPath.cubicTo(
      cx - r,
      size.height * 0.75,
      cx - r * 1.1,
      cy * 0.5,
      cx,
      size.height * 0.05,
    );
    canvas.drawPath(dropPath, paint);

    // Silhueta mãe (oval grande)
    canvas.drawOval(
      Rect.fromCenter(
        center: Offset(cx * 1.05, cy * 1.0),
        width: size.width * 0.45,
        height: size.height * 0.52,
      ),
      paint,
    );

    // Cabeça do bebê (círculo pequeno)
    canvas.drawCircle(
      Offset(cx * 0.72, cy * 0.72),
      size.width * 0.1,
      paint,
    );

    // Coração (simplificado como dois círculos + triângulo)
    final heartX = cx * 0.45;
    final heartY = cy * 1.1;
    final hR = size.width * 0.09;
    canvas.drawCircle(Offset(heartX - hR * 0.5, heartY), hR, fillPaint);
    canvas.drawCircle(Offset(heartX + hR * 0.5, heartY), hR, fillPaint);
    final heartBottom = Path();
    heartBottom.moveTo(heartX - hR * 1.1, heartY + hR * 0.3);
    heartBottom.lineTo(heartX, heartY + hR * 1.8);
    heartBottom.lineTo(heartX + hR * 1.1, heartY + hR * 0.3);
    canvas.drawPath(heartBottom, fillPaint);
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => false;
}

// ─────────────────────────────────────────
// MODELS
// ─────────────────────────────────────────
class Doadora {
  final String nome, endereco;
  bool visitada;
  Doadora({required this.nome, required this.endereco, this.visitada = false});
}

class Frasco {
  final String qrCode;
  final double volume;
  final DateTime coleta;
  final String? observacao;
  Frasco(
      {required this.qrCode,
      required this.volume,
      required this.coleta,
      this.observacao});
}

class VisitaDoadora {
  final Doadora doadora;
  DateTime? chegada;
  String? gps;
  double? temperatura;
  String? observacoes;
  List<Frasco> frascos = [];
  VisitaDoadora({required this.doadora});
}

class Rota {
  DateTime? saida;
  String? gpsInicial;
  double? temperaturaInicial;
  List<VisitaDoadora> visitas = [];
  DateTime? retorno;
  String? gpsFinal;
  double? temperaturaFinal;
  String? observacoesRetorno;
  int get totalFrascos => visitas.fold(0, (s, v) => s + v.frascos.length);
  int get visitadas => visitas.where((v) => v.chegada != null).length;
}

// ─────────────────────────────────────────
// SHARED WIDGETS
// ─────────────────────────────────────────
class AppBottomNav extends StatelessWidget {
  final int currentIndex;
  final Function(int) onTap;
  const AppBottomNav(
      {super.key, required this.currentIndex, required this.onTap});

  @override
  Widget build(BuildContext context) => Container(
        decoration: const BoxDecoration(
          color: AppTheme.surface,
          border: Border(top: BorderSide(color: AppTheme.border, width: 1)),
        ),
        child: BottomNavigationBar(
          currentIndex: currentIndex,
          onTap: onTap,
          backgroundColor: Colors.transparent,
          elevation: 0,
          selectedItemColor: AppTheme.primary,
          unselectedItemColor: AppTheme.textSecondary,
          selectedLabelStyle:
              const TextStyle(fontSize: 11, fontWeight: FontWeight.w600),
          unselectedLabelStyle: const TextStyle(fontSize: 11),
          items: const [
            BottomNavigationBarItem(
                icon: Icon(Icons.home_outlined),
                activeIcon: Icon(Icons.home),
                label: 'Início'),
            BottomNavigationBarItem(
                icon: Icon(Icons.add_road_outlined),
                activeIcon: Icon(Icons.add_road),
                label: 'Nova Rota'),
            BottomNavigationBarItem(
                icon: Icon(Icons.route_outlined),
                activeIcon: Icon(Icons.route),
                label: 'Rotas'),
          ],
        ),
      );
}

class SectionLabel extends StatelessWidget {
  final String text;
  const SectionLabel(this.text, {super.key});
  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.only(bottom: 8),
        child: Text(text,
            style: const TextStyle(
                fontSize: 12,
                fontWeight: FontWeight.w600,
                color: AppTheme.textSecondary,
                letterSpacing: 0.3)),
      );
}

class GpsCapturado extends StatelessWidget {
  final String coords;
  const GpsCapturado({super.key, required this.coords});
  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
        decoration: BoxDecoration(
          color: AppTheme.primaryLight,
          borderRadius: BorderRadius.circular(8),
          border: Border.all(color: AppTheme.primary.withOpacity(0.3)),
        ),
        child: Row(children: [
          const Icon(Icons.circle, color: AppTheme.gpsGreen, size: 9),
          const SizedBox(width: 8),
          Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
            const Text('GPS capturado',
                style: TextStyle(
                    fontSize: 11,
                    color: AppTheme.gpsGreen,
                    fontWeight: FontWeight.w700)),
            Text(coords,
                style:
                    const TextStyle(fontSize: 13, color: AppTheme.textPrimary)),
          ]),
          const Spacer(),
          const Icon(Icons.location_on, color: AppTheme.primary, size: 20),
        ]),
      );
}

class InfoCard extends StatelessWidget {
  final IconData icon;
  final String label, value;
  final Color? iconColor;
  const InfoCard(
      {super.key,
      required this.icon,
      required this.label,
      required this.value,
      this.iconColor});
  @override
  Widget build(BuildContext context) => Row(children: [
        Icon(icon, size: 16, color: iconColor ?? AppTheme.textSecondary),
        const SizedBox(width: 8),
        Text(label,
            style:
                const TextStyle(fontSize: 14, color: AppTheme.textSecondary)),
        const Spacer(),
        Text(value,
            style: const TextStyle(
                fontSize: 14,
                fontWeight: FontWeight.w600,
                color: AppTheme.textPrimary)),
      ]);
}

class DateTimeBox extends StatelessWidget {
  final String label;
  final IconData icon;
  const DateTimeBox(
      {super.key, required this.label, this.icon = Icons.access_time_outlined});
  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 14),
        decoration: BoxDecoration(
            color: AppTheme.surface,
            borderRadius: BorderRadius.circular(8),
            border: Border.all(color: AppTheme.border)),
        child: Row(children: [
          Expanded(
              child: Text(label,
                  style: const TextStyle(
                      fontSize: 14, color: AppTheme.textPrimary))),
          Icon(icon, color: AppTheme.textSecondary, size: 18),
        ]),
      );
}

class SectionTitle extends StatelessWidget {
  final String number, title;
  const SectionTitle({super.key, required this.number, required this.title});
  @override
  Widget build(BuildContext context) => Row(children: [
        Container(
          width: 22,
          height: 22,
          decoration: const BoxDecoration(
              color: AppTheme.primary, shape: BoxShape.circle),
          child: Center(
              child: Text(number,
                  style: const TextStyle(
                      color: Colors.white,
                      fontSize: 11,
                      fontWeight: FontWeight.w800))),
        ),
        const SizedBox(width: 8),
        Text(title,
            style: const TextStyle(
                fontSize: 14,
                fontWeight: FontWeight.w700,
                color: AppTheme.textPrimary)),
      ]);
}

class DoadoraAvatar extends StatelessWidget {
  final String nome;
  final double radius;
  const DoadoraAvatar({super.key, required this.nome, this.radius = 22});
  @override
  Widget build(BuildContext context) => CircleAvatar(
        radius: radius,
        backgroundColor: AppTheme.primaryLight,
        child: Text(nome[0],
            style: TextStyle(
                color: AppTheme.primary,
                fontWeight: FontWeight.w700,
                fontSize: radius * 0.8)),
      );
}

// ─────────────────────────────────────────
// SCREEN 1 — LOGIN
// ─────────────────────────────────────────
class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});
  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _usuarioCtrl = TextEditingController();
  final _senhaCtrl = TextEditingController();
  bool _senhaVis = false, _lembrar = false;

  @override
  Widget build(BuildContext context) => Scaffold(
        backgroundColor: AppTheme.background,
        body: SafeArea(
            child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 28),
          child: Column(children: [
            const Spacer(flex: 2),
            // Logo BLH Rastreabilidade
            Column(children: [
              SizedBox(
                width: 72,
                height: 72,
                child: CustomPaint(
                    painter: _BLHIconPainter(color: AppTheme.primary)),
              ),
              const SizedBox(height: 12),
              const Text('BLH',
                  style: TextStyle(
                      fontSize: 32,
                      fontWeight: FontWeight.w900,
                      color: AppTheme.primary,
                      letterSpacing: 2,
                      height: 1.0)),
              const Text('RASTREABILIDADE',
                  style: TextStyle(
                      fontSize: 12,
                      fontWeight: FontWeight.w600,
                      color: AppTheme.primary,
                      letterSpacing: 3)),
            ]),
            const SizedBox(height: 8),
            const Text('Banco de Leite Humano',
                style: TextStyle(fontSize: 13, color: AppTheme.textSecondary)),
            const Spacer(flex: 2),
            // Card de login estilo site
            Container(
              padding: const EdgeInsets.all(24),
              decoration: BoxDecoration(
                color: AppTheme.surface,
                borderRadius: BorderRadius.circular(16),
                border: Border.all(color: AppTheme.border),
                boxShadow: [
                  BoxShadow(
                      color: Colors.black.withOpacity(0.05),
                      blurRadius: 20,
                      offset: const Offset(0, 4))
                ],
              ),
              child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Text('Entrar',
                        style: TextStyle(
                            fontSize: 20,
                            fontWeight: FontWeight.w800,
                            color: AppTheme.textPrimary)),
                    const SizedBox(height: 4),
                    const Text('Acesse com suas credenciais',
                        style: TextStyle(
                            fontSize: 13, color: AppTheme.textSecondary)),
                    const SizedBox(height: 24),
                    TextField(
                        controller: _usuarioCtrl,
                        decoration: const InputDecoration(
                          prefixIcon: Icon(Icons.person_outline,
                              color: AppTheme.primary, size: 20),
                          hintText: 'Usuário',
                          labelText: 'Usuário',
                        )),
                    const SizedBox(height: 12),
                    TextField(
                      controller: _senhaCtrl,
                      obscureText: !_senhaVis,
                      decoration: InputDecoration(
                        prefixIcon: const Icon(Icons.lock_outline,
                            color: AppTheme.primary, size: 20),
                        labelText: 'Senha',
                        hintText: 'Senha',
                        suffixIcon: IconButton(
                          icon: Icon(
                              _senhaVis
                                  ? Icons.visibility_off_outlined
                                  : Icons.visibility_outlined,
                              color: AppTheme.textHint,
                              size: 20),
                          onPressed: () =>
                              setState(() => _senhaVis = !_senhaVis),
                        ),
                      ),
                    ),
                    const SizedBox(height: 8),
                    Row(children: [
                      Checkbox(
                        value: _lembrar,
                        onChanged: (v) => setState(() => _lembrar = v!),
                        activeColor: AppTheme.primary,
                        materialTapTargetSize: MaterialTapTargetSize.shrinkWrap,
                        side: const BorderSide(
                            color: AppTheme.border, width: 1.5),
                      ),
                      const Text('Lembrar meu acesso',
                          style: TextStyle(
                              fontSize: 13, color: AppTheme.textSecondary)),
                    ]),
                    const SizedBox(height: 16),
                    ElevatedButton(
                      onPressed: () => Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (_) => const IniciarRotaScreen())),
                      child: const Text('Entrar'),
                    ),
                  ]),
            ),
            const Spacer(flex: 3),
            // Rodapé
            Text('BLH Rastreabilidade © ${DateTime.now().year}',
                style: const TextStyle(fontSize: 11, color: AppTheme.textHint)),
            const SizedBox(height: 16),
          ]),
        )),
      );
}

// ─────────────────────────────────────────
// SCREEN 2 — INICIAR ROTA
// ─────────────────────────────────────────
class IniciarRotaScreen extends StatefulWidget {
  const IniciarRotaScreen({super.key});
  @override
  State<IniciarRotaScreen> createState() => _IniciarRotaScreenState();
}

class _IniciarRotaScreenState extends State<IniciarRotaScreen> {
  final _tempCtrl = TextEditingController(text: '4,2');
  final _rota = Rota();

  @override
  void initState() {
    super.initState();
    _rota.gpsInicial = '-23.550520, -46.633308';
    _rota.saida = DateTime(2025, 5, 22, 8, 30);
    _rota.visitas = [
      VisitaDoadora(
          doadora:
              Doadora(nome: 'Maria Silva', endereco: 'Rua das Flores, 123')),
      VisitaDoadora(
          doadora:
              Doadora(nome: 'Juliana Santos', endereco: 'Av. Brasil, 456')),
      VisitaDoadora(
          doadora: Doadora(nome: 'Ana Oliveira', endereco: 'Rua da Paz, 789')),
      VisitaDoadora(
          doadora:
              Doadora(nome: 'Carla Mendes', endereco: 'Av. Paulista, 1200')),
      VisitaDoadora(
          doadora: Doadora(
              nome: 'Fernanda Lima', endereco: 'Rua XV de Novembro, 45')),
    ];
  }

  @override
  Widget build(BuildContext context) => Scaffold(
        backgroundColor: AppTheme.background,
        appBar: AppBar(
          leading: const BackButton(color: AppTheme.primary),
          title: Row(children: [
            SizedBox(
              width: 28,
              height: 28,
              child: CustomPaint(
                  painter: _BLHIconPainter(color: AppTheme.primary)),
            ),
            const SizedBox(width: 8),
            const Text('Iniciar Nova Rota'),
          ]),
          actions: [
            IconButton(
                icon:
                    const Icon(Icons.bookmark_border, color: AppTheme.primary),
                onPressed: () {}),
          ],
          bottom: PreferredSize(
            preferredSize: const Size.fromHeight(1),
            child: Container(height: 1, color: AppTheme.border),
          ),
        ),
        bottomNavigationBar: AppBottomNav(currentIndex: 1, onTap: (_) {}),
        body: SingleChildScrollView(
          padding: const EdgeInsets.all(20),
          child:
              Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
            // Header card com status
            Container(
              padding: const EdgeInsets.all(16),
              decoration: BoxDecoration(
                color: AppTheme.primary,
                borderRadius: BorderRadius.circular(12),
              ),
              child: Row(children: [
                Container(
                  padding: const EdgeInsets.all(10),
                  decoration: BoxDecoration(
                    color: Colors.white.withOpacity(0.2),
                    borderRadius: BorderRadius.circular(10),
                  ),
                  child: const Icon(Icons.local_shipping_outlined,
                      color: Colors.white, size: 24),
                ),
                const SizedBox(width: 14),
                const Expanded(
                    child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                      Text('Nova Rota de Coleta',
                          style: TextStyle(
                              color: Colors.white,
                              fontSize: 15,
                              fontWeight: FontWeight.w700)),
                      Text('Preencha os dados de saída do BLH',
                          style:
                              TextStyle(color: Colors.white70, fontSize: 12)),
                    ])),
              ]),
            ),
            const SizedBox(height: 24),
            _SectionCard(
              title: 'Data / Hora saída do BLH',
              icon: Icons.calendar_today_outlined,
              child: const DateTimeBox(label: '22/05/2025  08:30'),
            ),
            const SizedBox(height: 16),
            _SectionCard(
              title: 'GPS inicial',
              icon: Icons.location_on_outlined,
              child: GpsCapturado(coords: _rota.gpsInicial!),
            ),
            const SizedBox(height: 16),
            _SectionCard(
              title: 'Temperatura inicial da caixa (°C)',
              icon: Icons.thermostat_outlined,
              child: TextField(
                controller: _tempCtrl,
                keyboardType:
                    const TextInputType.numberWithOptions(decimal: true),
                decoration:
                    const InputDecoration(hintText: '0,0', suffixText: '°C'),
              ),
            ),
            const SizedBox(height: 32),
            ElevatedButton.icon(
              icon: const Icon(Icons.play_arrow_rounded, size: 20),
              label: const Text('Salvar e Iniciar Rota'),
              onPressed: () {
                _rota.temperaturaInicial =
                    double.tryParse(_tempCtrl.text.replaceAll(',', '.'));
                Navigator.push(
                    context,
                    MaterialPageRoute(
                        builder: (_) => ListaDoadoras(rota: _rota)));
              },
            ),
          ]),
        ),
      );
}

class _SectionCard extends StatelessWidget {
  final String title;
  final IconData icon;
  final Widget child;
  const _SectionCard(
      {required this.title, required this.icon, required this.child});

  @override
  Widget build(BuildContext context) =>
      Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
        Row(children: [
          Icon(icon, size: 15, color: AppTheme.primary),
          const SizedBox(width: 6),
          Text(title,
              style: const TextStyle(
                  fontSize: 12,
                  fontWeight: FontWeight.w700,
                  color: AppTheme.textSecondary,
                  letterSpacing: 0.3)),
        ]),
        const SizedBox(height: 8),
        child,
      ]);
}

// ─────────────────────────────────────────
// SCREEN 3 — LISTA DE DOADORAS
// ─────────────────────────────────────────
class ListaDoadoras extends StatefulWidget {
  final Rota rota;
  const ListaDoadoras({super.key, required this.rota});
  @override
  State<ListaDoadoras> createState() => _ListaDoadoras();
}

class _ListaDoadoras extends State<ListaDoadoras>
    with SingleTickerProviderStateMixin {
  late TabController _tab;

  @override
  void initState() {
    super.initState();
    _tab = TabController(length: 2, vsync: this)
      ..addListener(() => setState(() {}));
  }

  List<VisitaDoadora> get pendentes =>
      widget.rota.visitas.where((v) => v.chegada == null).toList();
  List<VisitaDoadora> get visitadas =>
      widget.rota.visitas.where((v) => v.chegada != null).toList();

  @override
  Widget build(BuildContext context) => Scaffold(
        backgroundColor: AppTheme.background,
        appBar: AppBar(
          leading: const BackButton(color: AppTheme.primary),
          title:
              Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
            const Text('Rota em andamento',
                style: TextStyle(fontSize: 16, fontWeight: FontWeight.w700)),
            const Text('22/05/2025  •  Saída: 08:30',
                style: TextStyle(
                    fontSize: 11,
                    color: AppTheme.textSecondary,
                    fontWeight: FontWeight.w400)),
          ]),
          actions: [
            IconButton(
                icon: const Icon(Icons.more_vert, color: AppTheme.primary),
                onPressed: () {}),
          ],
          bottom: TabBar(
            controller: _tab,
            labelColor: AppTheme.primary,
            unselectedLabelColor: AppTheme.textSecondary,
            indicatorColor: AppTheme.primary,
            indicatorWeight: 3,
            labelStyle:
                const TextStyle(fontWeight: FontWeight.w600, fontSize: 13),
            tabs: [
              Tab(text: 'Pendentes (${pendentes.length})'),
              Tab(text: 'Visitadas (${visitadas.length})'),
            ],
          ),
        ),
        bottomNavigationBar: AppBottomNav(currentIndex: 1, onTap: (_) {}),
        body: TabBarView(controller: _tab, children: [
          _buildList(pendentes, isPendente: true),
          _buildList(visitadas, isPendente: false),
        ]),
        floatingActionButton: pendentes.isEmpty
            ? null
            : Padding(
                padding: const EdgeInsets.symmetric(horizontal: 20),
                child: SizedBox(
                    width: double.infinity,
                    child: OutlinedButton.icon(
                      icon: const Icon(Icons.person_add_outlined, size: 18),
                      label: const Text('Adicionar doadora não prevista'),
                      onPressed: () {},
                    )),
              ),
        floatingActionButtonLocation: FloatingActionButtonLocation.centerFloat,
      );

  Widget _buildList(List<VisitaDoadora> list, {required bool isPendente}) {
    if (list.isEmpty && !isPendente)
      return const Center(
          child: Text('Nenhuma visita realizada ainda.',
              style: TextStyle(color: AppTheme.textSecondary)));
    if (list.isEmpty && isPendente) return _AllVisitedBanner(rota: widget.rota);
    return ListView.separated(
      padding: const EdgeInsets.fromLTRB(16, 16, 16, 100),
      itemCount: list.length,
      separatorBuilder: (_, __) => const SizedBox(height: 8),
      itemBuilder: (ctx, i) {
        final v = list[i];
        final isDone = v.chegada != null;
        return InkWell(
          onTap: () async {
            await Navigator.push(
                ctx,
                MaterialPageRoute(
                    builder: (_) =>
                        VisitaDoadoraScreen(visita: v, rota: widget.rota)));
            setState(() {});
          },
          borderRadius: BorderRadius.circular(12),
          child: Container(
            padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 14),
            decoration: BoxDecoration(
              color: AppTheme.surface,
              borderRadius: BorderRadius.circular(12),
              border: Border.all(
                  color: isDone
                      ? AppTheme.primary.withOpacity(0.3)
                      : AppTheme.border),
              boxShadow: [
                BoxShadow(
                    color: Colors.black.withOpacity(0.03),
                    blurRadius: 8,
                    offset: const Offset(0, 2))
              ],
            ),
            child: Row(children: [
              Stack(children: [
                DoadoraAvatar(nome: v.doadora.nome),
                if (isDone)
                  Positioned(
                      right: 0,
                      bottom: 0,
                      child: Container(
                        width: 14,
                        height: 14,
                        decoration: const BoxDecoration(
                            color: AppTheme.success, shape: BoxShape.circle),
                        child: const Icon(Icons.check,
                            color: Colors.white, size: 9),
                      )),
              ]),
              const SizedBox(width: 12),
              Expanded(
                  child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                    Text(v.doadora.nome,
                        style: const TextStyle(
                            fontSize: 14,
                            fontWeight: FontWeight.w600,
                            color: AppTheme.textPrimary)),
                    const SizedBox(height: 2),
                    Row(children: [
                      const Icon(Icons.location_on_outlined,
                          size: 12, color: AppTheme.textSecondary),
                      const SizedBox(width: 2),
                      Text(v.doadora.endereco,
                          style: const TextStyle(
                              fontSize: 12, color: AppTheme.textSecondary)),
                    ]),
                    if (isDone && v.frascos.isNotEmpty) ...[
                      const SizedBox(height: 4),
                      Container(
                        padding: const EdgeInsets.symmetric(
                            horizontal: 8, vertical: 2),
                        decoration: BoxDecoration(
                          color: AppTheme.primaryLight,
                          borderRadius: BorderRadius.circular(20),
                        ),
                        child: Text('${v.frascos.length} frasco(s)',
                            style: const TextStyle(
                                fontSize: 11,
                                color: AppTheme.primary,
                                fontWeight: FontWeight.w600)),
                      ),
                    ],
                  ])),
              const Icon(Icons.chevron_right, color: AppTheme.textHint),
            ]),
          ),
        );
      },
    );
  }
}

class _AllVisitedBanner extends StatelessWidget {
  final Rota rota;
  const _AllVisitedBanner({required this.rota});
  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.all(24),
        child: Column(mainAxisAlignment: MainAxisAlignment.center, children: [
          Container(
              width: 80,
              height: 80,
              decoration: const BoxDecoration(
                  color: AppTheme.successLight, shape: BoxShape.circle),
              child: const Icon(Icons.check_circle,
                  color: AppTheme.success, size: 48)),
          const SizedBox(height: 24),
          const Text('Todas as doadoras\nforam visitadas.',
              textAlign: TextAlign.center,
              style: TextStyle(
                  fontSize: 20,
                  fontWeight: FontWeight.w800,
                  color: AppTheme.textPrimary)),
          const SizedBox(height: 8),
          const Text('Deseja encerrar a rota e registrar retorno ao BLH?',
              textAlign: TextAlign.center,
              style: TextStyle(fontSize: 14, color: AppTheme.textSecondary)),
          const SizedBox(height: 32),
          ElevatedButton(
            onPressed: () => Navigator.push(
                context,
                MaterialPageRoute(
                    builder: (_) => RetornoBLHScreen(rota: rota))),
            child: const Text('Sim, encerrar rota'),
          ),
          const SizedBox(height: 12),
          OutlinedButton(
              onPressed: () {}, child: const Text('Não, revisar rota')),
        ]),
      );
}

// ─────────────────────────────────────────
// SCREEN 4 — VISITA À DOADORA
// ─────────────────────────────────────────
class VisitaDoadoraScreen extends StatefulWidget {
  final VisitaDoadora visita;
  final Rota rota;
  const VisitaDoadoraScreen(
      {super.key, required this.visita, required this.rota});
  @override
  State<VisitaDoadoraScreen> createState() => _VisitaDoadoraScreenState();
}

class _VisitaDoadoraScreenState extends State<VisitaDoadoraScreen> {
  final _tempCtrl = TextEditingController(text: '3,8');
  final _obsCtrl = TextEditingController(
      text: 'Doadora em bom estado.\nSem intercorrências.');
  bool _chegadaOk = false, _mostrarFrascos = false;

  @override
  Widget build(BuildContext context) => Scaffold(
        backgroundColor: AppTheme.background,
        appBar: AppBar(
          leading: const BackButton(color: AppTheme.primary),
          title: const Text('Visita à Doadora'),
          actions: [
            IconButton(
                icon: const Icon(Icons.more_vert, color: AppTheme.primary),
                onPressed: () {}),
          ],
        ),
        body: SingleChildScrollView(
            padding: const EdgeInsets.all(20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                // Header doadora
                Container(
                  padding: const EdgeInsets.all(16),
                  decoration: BoxDecoration(
                    color: AppTheme.surface,
                    borderRadius: BorderRadius.circular(12),
                    border: Border.all(color: AppTheme.border),
                    boxShadow: [
                      BoxShadow(
                          color: Colors.black.withOpacity(0.04), blurRadius: 8)
                    ],
                  ),
                  child: Row(children: [
                    DoadoraAvatar(nome: widget.visita.doadora.nome, radius: 24),
                    const SizedBox(width: 12),
                    Expanded(
                        child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                          Text(widget.visita.doadora.nome,
                              style: const TextStyle(
                                  fontSize: 16,
                                  fontWeight: FontWeight.w700,
                                  color: AppTheme.textPrimary)),
                          Row(children: [
                            const Icon(Icons.location_on_outlined,
                                size: 13, color: AppTheme.textSecondary),
                            const SizedBox(width: 2),
                            Text(widget.visita.doadora.endereco,
                                style: const TextStyle(
                                    fontSize: 12,
                                    color: AppTheme.textSecondary)),
                          ]),
                        ])),
                    Container(
                      padding: const EdgeInsets.symmetric(
                          horizontal: 10, vertical: 6),
                      decoration: BoxDecoration(
                        color: AppTheme.primaryLight,
                        borderRadius: BorderRadius.circular(20),
                      ),
                      child: const Row(children: [
                        Icon(Icons.map_outlined,
                            size: 13, color: AppTheme.primary),
                        SizedBox(width: 4),
                        Text('Mapa',
                            style: TextStyle(
                                fontSize: 12,
                                color: AppTheme.primary,
                                fontWeight: FontWeight.w600)),
                      ]),
                    ),
                  ]),
                ),
                const SizedBox(height: 24),

                // Seção 1 — Chegada
                const SectionTitle(number: '1', title: 'Chegada'),
                const SizedBox(height: 12),
                const SectionLabel('Registrar chegada'),
                const DateTimeBox(label: '22/05/2025  09:10'),
                const SizedBox(height: 14),
                const SectionLabel('GPS da visita'),
                const GpsCapturado(coords: '-23.551000, -46.633700'),
                const SizedBox(height: 24),

                // Seção 2 — Temperatura
                const SectionTitle(number: '2', title: 'Temperatura'),
                const SizedBox(height: 12),
                const SectionLabel('Temperatura da caixa na chegada (°C)'),
                TextField(
                  controller: _tempCtrl,
                  keyboardType:
                      const TextInputType.numberWithOptions(decimal: true),
                  decoration:
                      const InputDecoration(hintText: '0,0', suffixText: '°C'),
                ),
                const SizedBox(height: 24),

                if (!_chegadaOk)
                  ElevatedButton.icon(
                    icon: const Icon(Icons.check_circle_outline, size: 18),
                    label: const Text('Confirmar chegada e continuar'),
                    onPressed: () => setState(() {
                      _chegadaOk = _mostrarFrascos = true;
                      widget.visita.chegada = DateTime.now();
                    }),
                  ),

                if (_mostrarFrascos) ...[
                  const SectionTitle(number: '3', title: 'Frascos coletados'),
                  const SizedBox(height: 12),
                  ...widget.visita.frascos.map((f) => _FrascoItem(frasco: f)),
                  OutlinedButton.icon(
                    icon: const Icon(Icons.add, size: 18),
                    label: const Text('Adicionar frasco'),
                    onPressed: () => _showFrascoSheet(context),
                  ),
                  const SizedBox(height: 24),
                  const SectionTitle(number: '4', title: 'Observações gerais'),
                  const SizedBox(height: 12),
                  TextField(
                    controller: _obsCtrl,
                    maxLines: 3,
                    decoration: const InputDecoration(
                        hintText: 'Ex.: Dificuldade na coleta...'),
                  ),
                  const SizedBox(height: 24),
                  ElevatedButton.icon(
                    icon: const Icon(Icons.save_outlined, size: 18),
                    label: const Text('Salvar visita'),
                    onPressed: () => _salvar(context),
                  ),
                  const SizedBox(height: 12),
                ],
              ],
            )),
      );

  void _showFrascoSheet(BuildContext context) {
    final volCtrl = TextEditingController(text: '150');
    final obsCtrl = TextEditingController();
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: AppTheme.surface,
      shape: const RoundedRectangleBorder(
          borderRadius: BorderRadius.vertical(top: Radius.circular(20))),
      builder: (ctx) => Padding(
        padding: EdgeInsets.only(
            bottom: MediaQuery.of(ctx).viewInsets.bottom + 20,
            left: 20,
            right: 20,
            top: 20),
        child: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(children: [
                Container(
                  width: 40,
                  height: 4,
                  margin: const EdgeInsets.only(bottom: 16),
                  decoration: BoxDecoration(
                      color: AppTheme.border,
                      borderRadius: BorderRadius.circular(2)),
                ),
              ]),
              Row(children: [
                const Text('Adicionar frasco',
                    style: TextStyle(
                        fontSize: 17,
                        fontWeight: FontWeight.w700,
                        color: AppTheme.textPrimary)),
                const Spacer(),
                IconButton(
                    icon: const Icon(Icons.close),
                    onPressed: () => Navigator.pop(ctx)),
              ]),
              const SizedBox(height: 16),
              Row(children: [
                Expanded(
                    child: OutlinedButton.icon(
                        icon: const Icon(Icons.qr_code_scanner, size: 18),
                        label: const Text('Ler QR Code'),
                        onPressed: () {})),
                const SizedBox(width: 12),
                Expanded(
                    child: OutlinedButton.icon(
                        icon: const Icon(Icons.qr_code, size: 18),
                        label: const Text('Gerar QR Code'),
                        onPressed: () {})),
              ]),
              const SizedBox(height: 16),
              const SectionLabel('Volume (ml)'),
              TextField(
                  controller: volCtrl,
                  keyboardType: TextInputType.number,
                  decoration:
                      const InputDecoration(hintText: '0', suffixText: 'ml')),
              const SizedBox(height: 12),
              const SectionLabel('Data/Hora da coleta'),
              const DateTimeBox(label: '22/05/2025  09:15'),
              const SizedBox(height: 12),
              const SectionLabel('Observações (opcional)'),
              TextField(
                  controller: obsCtrl,
                  decoration: const InputDecoration(
                      hintText: 'Ex.: Dificuldade na coleta...')),
              const SizedBox(height: 20),
              ElevatedButton(
                onPressed: () {
                  setState(() {
                    widget.visita.frascos.add(Frasco(
                      qrCode:
                          'QR${DateTime.now().millisecondsSinceEpoch % 100000}',
                      volume: double.tryParse(volCtrl.text) ?? 0,
                      coleta: DateTime.now(),
                      observacao: obsCtrl.text.isNotEmpty ? obsCtrl.text : null,
                    ));
                  });
                  Navigator.pop(ctx);
                },
                child: const Text('Salvar frasco'),
              ),
            ]),
      ),
    );
  }

  void _salvar(BuildContext context) {
    widget.visita.temperatura =
        double.tryParse(_tempCtrl.text.replaceAll(',', '.'));
    widget.visita.observacoes = _obsCtrl.text;
    showDialog(
      context: context,
      builder: (ctx) => AlertDialog(
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
        content: Column(mainAxisSize: MainAxisSize.min, children: [
          Container(
              width: 70,
              height: 70,
              decoration: const BoxDecoration(
                  color: AppTheme.successLight, shape: BoxShape.circle),
              child: const Icon(Icons.check_circle,
                  color: AppTheme.success, size: 42)),
          const SizedBox(height: 16),
          const Text('Visita salva com sucesso!',
              style: TextStyle(
                  fontSize: 18,
                  fontWeight: FontWeight.w700,
                  color: AppTheme.textPrimary)),
          const SizedBox(height: 6),
          const Text('Deseja ir para a próxima doadora?',
              textAlign: TextAlign.center,
              style: TextStyle(fontSize: 14, color: AppTheme.textSecondary)),
          const SizedBox(height: 20),
          ElevatedButton(
              onPressed: () {
                Navigator.pop(ctx);
                Navigator.pop(context);
              },
              child: const Text('Próxima doadora')),
          const SizedBox(height: 8),
          TextButton(
              onPressed: () {
                Navigator.pop(ctx);
                Navigator.pop(context);
              },
              child: const Text('Voltar para a lista',
                  style: TextStyle(color: AppTheme.textSecondary))),
        ]),
      ),
    );
  }
}

class _FrascoItem extends StatelessWidget {
  final Frasco frasco;
  const _FrascoItem({required this.frasco});
  @override
  Widget build(BuildContext context) => Container(
        margin: const EdgeInsets.only(bottom: 8),
        padding: const EdgeInsets.all(14),
        decoration: BoxDecoration(
            color: AppTheme.surface,
            borderRadius: BorderRadius.circular(10),
            border: Border.all(color: AppTheme.border)),
        child: Row(children: [
          Container(
            padding: const EdgeInsets.all(8),
            decoration: BoxDecoration(
                color: AppTheme.primaryLight,
                borderRadius: BorderRadius.circular(8)),
            child: const Icon(Icons.science_outlined,
                color: AppTheme.primary, size: 22),
          ),
          const SizedBox(width: 12),
          Expanded(
              child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                Text('Frasco ${frasco.qrCode}',
                    style: const TextStyle(
                        fontSize: 13,
                        fontWeight: FontWeight.w600,
                        color: AppTheme.textPrimary)),
                Text(
                    '${frasco.volume.toStringAsFixed(0)} ml  •  '
                    '${frasco.coleta.hour.toString().padLeft(2, "0")}:${frasco.coleta.minute.toString().padLeft(2, "0")}',
                    style: const TextStyle(
                        fontSize: 12, color: AppTheme.textSecondary)),
              ])),
          const Icon(Icons.chevron_right, color: AppTheme.textHint),
        ]),
      );
}

// ─────────────────────────────────────────
// SCREEN 9 — RETORNO AO BLH
// ─────────────────────────────────────────
class RetornoBLHScreen extends StatefulWidget {
  final Rota rota;
  const RetornoBLHScreen({super.key, required this.rota});
  @override
  State<RetornoBLHScreen> createState() => _RetornoBLHScreenState();
}

class _RetornoBLHScreenState extends State<RetornoBLHScreen> {
  final _tempCtrl = TextEditingController(text: '4,1');
  final _obsCtrl = TextEditingController();

  @override
  Widget build(BuildContext context) => Scaffold(
        backgroundColor: AppTheme.background,
        appBar: AppBar(
            leading: const BackButton(color: AppTheme.primary),
            title: const Text('Retorno ao BLH')),
        body: SingleChildScrollView(
            padding: const EdgeInsets.all(20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                // Resumo da rota
                Container(
                  padding: const EdgeInsets.all(16),
                  decoration: BoxDecoration(
                    color: AppTheme.primaryLight,
                    borderRadius: BorderRadius.circular(12),
                    border:
                        Border.all(color: AppTheme.primary.withOpacity(0.2)),
                  ),
                  child: Row(children: [
                    Expanded(
                        child: _StatBox(
                            label: 'Doadoras',
                            value: '${widget.rota.visitadas}',
                            icon: Icons.people_outline)),
                    Container(
                        width: 1,
                        height: 40,
                        color: AppTheme.primary.withOpacity(0.2)),
                    Expanded(
                        child: _StatBox(
                            label: 'Frascos',
                            value: '${widget.rota.totalFrascos}',
                            icon: Icons.science_outlined)),
                  ]),
                ),
                const SizedBox(height: 24),
                _SectionCard(
                  title: 'Registrar retorno ao BLH',
                  icon: Icons.flag_outlined,
                  child: const DateTimeBox(label: '22/05/2025  12:45'),
                ),
                const SizedBox(height: 16),
                _SectionCard(
                  title: 'GPS final',
                  icon: Icons.location_on_outlined,
                  child: const GpsCapturado(coords: '-23.550100, -46.633100'),
                ),
                const SizedBox(height: 16),
                _SectionCard(
                  title: 'Temperatura final da caixa (°C)',
                  icon: Icons.thermostat_outlined,
                  child: TextField(
                    controller: _tempCtrl,
                    keyboardType:
                        const TextInputType.numberWithOptions(decimal: true),
                    decoration: const InputDecoration(
                        hintText: '0,0', suffixText: '°C'),
                  ),
                ),
                const SizedBox(height: 16),
                _SectionCard(
                  title: 'Observações (opcional)',
                  icon: Icons.notes_outlined,
                  child: TextField(
                    controller: _obsCtrl,
                    maxLines: 3,
                    decoration: const InputDecoration(
                        hintText: 'Alguma observação sobre o retorno...'),
                  ),
                ),
                const SizedBox(height: 32),
                ElevatedButton.icon(
                  icon: const Icon(Icons.check_circle_outline, size: 18),
                  label: const Text('Finalizar Rota'),
                  onPressed: () {
                    widget.rota.retorno = DateTime(2025, 5, 22, 12, 45);
                    widget.rota.gpsFinal = '-23.550100, -46.633100';
                    widget.rota.temperaturaFinal =
                        double.tryParse(_tempCtrl.text.replaceAll(',', '.'));
                    Navigator.push(
                        context,
                        MaterialPageRoute(
                            builder: (_) =>
                                RotaConcluidaScreen(rota: widget.rota)));
                  },
                ),
              ],
            )),
      );
}

class _StatBox extends StatelessWidget {
  final String label, value;
  final IconData icon;
  const _StatBox(
      {required this.label, required this.value, required this.icon});
  @override
  Widget build(BuildContext context) => Column(children: [
        Icon(icon, color: AppTheme.primary, size: 22),
        const SizedBox(height: 4),
        Text(value,
            style: const TextStyle(
                fontSize: 22,
                fontWeight: FontWeight.w800,
                color: AppTheme.primary)),
        Text(label,
            style:
                const TextStyle(fontSize: 12, color: AppTheme.textSecondary)),
      ]);
}

// ─────────────────────────────────────────
// SCREEN 10 — ROTA CONCLUÍDA
// ─────────────────────────────────────────
class RotaConcluidaScreen extends StatelessWidget {
  final Rota rota;
  const RotaConcluidaScreen({super.key, required this.rota});

  String _fmt(DateTime? dt) => dt == null
      ? '--'
      : '${dt.hour.toString().padLeft(2, "0")}:${dt.minute.toString().padLeft(2, "0")}';

  @override
  Widget build(BuildContext context) => Scaffold(
        backgroundColor: AppTheme.background,
        body: SafeArea(
            child: Column(children: [
          // Header com gradiente verde BLH
          Container(
            width: double.infinity,
            padding: const EdgeInsets.symmetric(vertical: 40, horizontal: 24),
            decoration: const BoxDecoration(
              gradient: LinearGradient(colors: [
                AppTheme.primaryDark,
                AppTheme.primary,
                AppTheme.accent
              ], begin: Alignment.topLeft, end: Alignment.bottomRight),
            ),
            child: Column(children: [
              // Logo no topo
              Row(mainAxisAlignment: MainAxisAlignment.center, children: [
                SizedBox(
                  width: 32,
                  height: 32,
                  child: CustomPaint(
                      painter: _BLHIconPainter(color: Colors.white)),
                ),
                const SizedBox(width: 8),
                const Text('BLH',
                    style: TextStyle(
                        fontSize: 20,
                        fontWeight: FontWeight.w900,
                        color: Colors.white,
                        letterSpacing: 2)),
              ]),
              const SizedBox(height: 20),
              Container(
                  width: 72,
                  height: 72,
                  decoration: BoxDecoration(
                      color: Colors.white.withOpacity(0.2),
                      shape: BoxShape.circle),
                  child: const Icon(Icons.check_circle,
                      color: Colors.white, size: 48)),
              const SizedBox(height: 16),
              const Text('Rota concluída!',
                  style: TextStyle(
                      fontSize: 24,
                      fontWeight: FontWeight.w800,
                      color: Colors.white)),
              const SizedBox(height: 4),
              const Text('Dados salvos e prontos para sincronizar.',
                  textAlign: TextAlign.center,
                  style: TextStyle(fontSize: 13, color: Colors.white70)),
            ]),
          ),
          Expanded(
              child: SingleChildScrollView(
            padding: const EdgeInsets.all(20),
            child:
                Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
              const Text('Resumo da rota',
                  style: TextStyle(
                      fontSize: 15,
                      fontWeight: FontWeight.w700,
                      color: AppTheme.textPrimary)),
              const SizedBox(height: 16),
              Container(
                padding: const EdgeInsets.all(16),
                decoration: BoxDecoration(
                  color: AppTheme.surface,
                  borderRadius: BorderRadius.circular(12),
                  border: Border.all(color: AppTheme.border),
                  boxShadow: [
                    BoxShadow(
                        color: Colors.black.withOpacity(0.04),
                        blurRadius: 12,
                        offset: const Offset(0, 4))
                  ],
                ),
                child: Column(children: [
                  InfoCard(
                      icon: Icons.calendar_today,
                      label: 'Data',
                      value: '22/05/2025'),
                  const Divider(height: 20, color: AppTheme.border),
                  InfoCard(icon: Icons.login, label: 'Saída', value: '08:30'),
                  const Divider(height: 20, color: AppTheme.border),
                  InfoCard(
                      icon: Icons.logout,
                      label: 'Retorno',
                      value: _fmt(rota.retorno)),
                  const Divider(height: 20, color: AppTheme.border),
                  InfoCard(
                      icon: Icons.people,
                      label: 'Doadoras visitadas',
                      value: '${rota.visitadas}',
                      iconColor: AppTheme.primary),
                  const Divider(height: 20, color: AppTheme.border),
                  InfoCard(
                      icon: Icons.science,
                      label: 'Frascos coletados',
                      value: '${rota.totalFrascos}',
                      iconColor: AppTheme.primary),
                ]),
              ),
              const SizedBox(height: 24),
              OutlinedButton.icon(
                icon: const Icon(Icons.description_outlined, size: 18),
                label: const Text('Ver resumo detalhado'),
                onPressed: () {},
              ),
              const SizedBox(height: 12),
              ElevatedButton.icon(
                icon: const Icon(Icons.route, size: 18),
                label: const Text('Ir para rotas'),
                onPressed: () =>
                    Navigator.of(context).popUntil((r) => r.isFirst),
              ),
            ]),
          )),
        ])),
      );
}

// ─────────────────────────────────────────
// MAIN
// ─────────────────────────────────────────
void main() => runApp(const ColetaLeiteApp());

class ColetaLeiteApp extends StatelessWidget {
  const ColetaLeiteApp({super.key});
  @override
  Widget build(BuildContext context) => MaterialApp(
        title: 'BLH Rastreabilidade',
        debugShowCheckedModeBanner: false,
        theme: AppTheme.theme,
        home: const LoginScreen(),
      );
}
