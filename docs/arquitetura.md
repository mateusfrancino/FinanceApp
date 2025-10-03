📖 Arquitetura do FinanceApp

Este documento descreve a arquitetura do FinanceApp, um aplicativo desenvolvido em .NET MAUI (.NET 9) com fins didáticos, simulando séries financeiras com Movimento Browniano Geométrico.

🎯 Objetivos

Explorar boas práticas de arquitetura em .NET MAUI;

Aplicar o padrão MVVM (Model-View-ViewModel);

Demonstrar renderização gráfica com SkiaSharp;

Separar responsabilidades entre camadas (UI, ViewModel, Services, Helpers).

🏗️ Estrutura em Camadas
FinanceApp/

├── Models/              # Estruturas de dados simples

├── Services/            # Regras de negócio / simulação

├── ViewModels/          # Lógica de apresentação (MVVM)

├── Views/               # Interface com o usuário (UI)

├── Helpers/             # Funções auxiliares reutilizáveis

🔄 Fluxo de Execução

Usuário interage na UI (Views)

Insere parâmetros (Preço inicial, Volatilidade, Média, Dias, Simulações).

Seleciona cor, estilo de linha, espessura, etc.

ViewModel processa a entrada

Comandos (ICommand) como SimulateCommand disparam a lógica.

Chama BrownianSimulator para gerar os dados.

Atualiza a coleção Series (observable) que notifica a View.

Service gera os dados

BrownianSimulator.GenerateMany() cria séries com base no Movimento Browniano Geométrico.

Cada série é retornada como array de double[].

View renderiza com SkiaSharp

O evento OnPaintSurface consome as séries.

Desenha eixos, escalas, ticks e curvas no SKCanvas.

O estilo (cor, espessura, tracejado) vem do ViewModel.

📊 Decisões Técnicas

MVVM foi adotado para separar UI de lógica de negócios, permitindo testes e reutilização.

SkiaSharp foi escolhido em vez de bibliotecas prontas de gráficos para:

Controle total do desenho;

Aprendizado de renderização em baixo nível;

Flexibilidade na personalização visual.

Helpers isolados garantem que a matemática de escalas não “polua” o code-behind da View.

Services encapsulam a simulação, evitando regras de negócio no ViewModel.

📈 Extensões Futuras

Criar testes unitários para validar cálculos do BrownianSimulator e do MathHelpers.

Adicionar salvamento/exportação de gráficos (ex.: PNG/PDF).

Permitir zoom/pan interativo no gráfico.

Criar múltiplas telas (ex.: histórico de simulações).
