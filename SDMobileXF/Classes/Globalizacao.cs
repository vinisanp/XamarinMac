using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Xamarin.Forms;

namespace SDMobileXF.Classes
{
    public class Globalizacao
    {
        private static HashSet<string> _log = new HashSet<string>();
        private static List<Idioma> _idiomas = new List<Idioma>();

        public static string CodigoIdiomaAtual { get; set; }

        public static CultureInfo Cultura
        {
            get
            {
                return CultureInfo.GetCultureInfo(Globalizacao.CodigoIdiomaAtual);
            }
        }

        public static string FormatoData
        {
            get
            {
                return Globalizacao.Cultura.DateTimeFormat.ShortDatePattern;
            }
        }

        public static string FormatoHora
        {
            get
            {
                return Globalizacao.Cultura.DateTimeFormat.LongTimePattern;
            }
        }

        public static List<Idioma> Idiomas
        {
            get
            {
                if (!_idiomas.Any())
                {
                    _idiomas.Add(new Idioma("pt", "Português", "Portugues.png"));
                    _idiomas.Add(new Idioma("en", "English", "Ingles.png"));
                    _idiomas.Add(new Idioma("es", "Español", "Espanhol.png"));
                    _idiomas.Add(new Idioma("fr", "Français", "Frances.png"));
                }

                return _idiomas;
            }
        }

        public static Idioma IdiomaAtual
        {
            get
            {
                return Idiomas.FirstOrDefault(i => i.Codigo == CodigoIdiomaAtual);
            }
        }

        public static string Traduz(string texto)
        {
            if (CodigoIdiomaAtual == "en")
                return TraduzEn(texto);
            else if (CodigoIdiomaAtual == "es")
                return TraduzEs(texto);
            else if (CodigoIdiomaAtual == "fr")
                return TraduzFr(texto);
            else
                return texto;
        }

        private static string TraduzEn(string texto)
        {
            switch(texto)
            {
                case "Acompanhe suas Ocorrências": return "Track your Events";
                case "Ajustes das configurações": return "Settings adjustments";
                case "Ajustes serão aplicados na próxima execução!": return "Adjustments will be applied in the next run!";
                case "Atualizando tabelas, progresso: ": return "Updating tables, progress: ";
                case "Aviso": return "Notice";
                case "Ações Tomadas": return "Actions taken";
                case "Bem-vindo": return "Welcome";
                case "Campos obrigatórios": return "Required fields";
                case "Cancelar Registro": return "Cancel Registration";
                case "Carregando...": return "Loading...";
                case "Categoria da Ocorrência": return "Occurrence Category";
                case "Claro": return "Light";
                case "Classificação da Ocorrência": return "Occurrence Classification";
                case "Comunicado por": return "Communicated by";
                case "Configurações": return "Settings";
                case "S.T. Suzano - Configurações": return "Security Suzano - Settings";
                case "S.T. Suzano": return "Security Suzano";
                case "DNA - De Olho na Área": return "DNA - Keeping an eye on the area";
                case "S.T. Suzano - Informações": return "Security Suzano - Information";
                case "S.T. Suzano - Ocorrência": return "Security Suzano - Occurrence";
                case "S.T. Suzano - Ocorrências": return "Security Suzano - Occurrences";
                case "Dados da Ocorrência": return "Occurrence Data";
                case "Dados não salvos serão perdidos. Deseja continuar?": return "Unsaved data will be lost. Do you wish to continue?";
                case "Data Inicial > Data Final.": return "Start Date > End Date.";
                case "Data": return "Date";
                case "De Olho na Área": return "Keeping an eye on the area";
                case "Descrever a ação imediata": return "Describe immediate action";
                case "Descrição da Ocorrência": return "Occurrence description";
                case "Digite ao menos 3 caracteres para efetuar o filtro!": return "Enter at least 3 characters to perform the filter!";
                case "Digite seu usuário e senha": return "Enter your username and password";
                case "Efetuar log out": return "Log out";
                case "Entrar": return "Log in";
                case "Erro ao salvar!": return "Error saving!";
                case "Erro": return "Error";
                case "Escuro": return "Dark";
                case "Estilo": return "Style";
                case "Foram Tomadas Ações Imediatas?": return "Were Immediate Actions Taken?";
                case "Fornecedor": return "Provider";
                case "Gerência do desvio": return "Deviation management";
                case "Grande": return "Large";
                case "Hora": return "Hour";
                case "Identificação": return "Identification";
                case "Idioma": return "Language";
                case "Informações sobre o aplicativo": return "Application information";
                case "Kbytes baixados: ": return "Downloaded Kbytes: ";
                case "Lembrar me": return "Remember me";
                case "Lembrar-me": return "Remember me";
                case "Limpar Dados": return "Clear Data";
                case "Local do desvio": return "Deviation location";
                case "Localização": return "Location";
                case "Médio": return "Medium";
                case "Nenhum registro encontrado com o filtro informado!": return "No records found with the given filter!";
                case "Não existem registros para o período selecionado.": return "There are no records for the selected period.";
                case "Não quero me identificar": return "I don't want to identify myself";
                case "Não": return "Not";
                case "Número": return "Number";
                case "Ok": return "Ok";
                case "Pequeno": return "Small";
                case "Registrado Por": return "Registered By";
                case "Registrar Ocorrência": return "Record Occurrence";
                case "Registrar ocorrências": return "Log occurrences";
                case "Responsável pela Ocorrência": return "Responsible for the Occurrence";
                case "Sair": return "Exit";
                case "Selecione a gerência do desvio!": return "Select bypass management!";
                case "Selecione a unidade do desvio!": return "Select the unit of the deviation!";
                case "Selecione a área do desvio!": return "Select the deviation area!";
                case "Senha": return "Password";
                case "Sim": return "Yes";
                case "Sincronizar": return "Synchronize";
                case "Sincronização": return "Synchronization";
                case "Sobre": return "About";
                case "Subclassificação da Ocorrência": return "Subclassification of Occurrence";
                case "Tamanho da fonte": return "Font size";
                case "Tipo de Ocorrência": return "Occurrence Type";
                case "Total de registros baixados: ": return "Total records downloaded: ";
                case "Unidade do desvio": return "Deviation unit";
                case "Usuário ou senha inválidos": return "Username or password is invalid";
                case "Usuário": return "Username";
                case "Ver ocorrências cadastradas": return "View registered occurrences";
                case "Voltar para a tela inicial": return "Return to the home screen";
                case "Área do desvio": return "Deviation area";
                case "Última atualização das tabelas: ": return "Tables last updated: ";
                case "Mais de 1000 registros encontrados.": return "More than 1000 records found.";
                case "Deseja continuar assim mesmo?": return "Do you want to continue anyway?";
                case "Refine a sua pesquisa para evitar travamentos.": return "Refine your search to avoid crashes.";
                case "Ocorrência cadastrada com sucesso!": return "Occurrences saved successfully!";
                case "Data Final": return "Final date";
                case "Data Inicial": return "Initial date";
                case "Não foi possível acessar a lista de ocorrências.": return "Could not access the hit list.";
                case "Ocorrências Cadastradas": return "Registered occurrences";
                case "Ocorrências a serem enviadas": return "Occurrences to be sent";
                case "Selecione o período": return "Select the period";
                case "Verifique se o dispositivo possui conexão com a internet.": return "Check that the device has an internet connection.";
                case "Cadastradas": return "Registered";
                case "Pendentes": return "Pending";
                case "Ocorrência '{0}' cadastrada com sucesso!": return "Occurrences '{0}' saved successfully!";
                case "Erro ao sincronizar.": return "Error while syncing.";
                case "Mais de 1000 registros encontrados. Refine a sua pesquisa.": return "More than 1000 records found. Refine your search.";
                case "Não foi possível obter os dados da ocorrência.": return "It was not possible to obtain the occurrence data.";
                case "Não foi possível sincronizar.": return "Couldn't sync.";
                case "Selecione classificação da ocorrência!": return "Select occurrence classification!";
                case "Selecione subclassificação da ocorrência!": return "Select subclassification of the occurrence!";
                case "Tente novamente mais tarde.": return "Try again later.";
                case "A - Equipamento de Proteção Individual e Coletivo": return "A - Individual and Collective Protection Equipment";
                case "Ativadores de Comportamento": return "Behavior Activators";
                case "Ações em Emergência (Ex.: Rota de Fuga, Chuveiro, Telefone, Brigadista, Alergia)": return "Emergency Actions (Ex. Escape Route, Shower, Telephone, Brigade, Allergy)";
                case "B - Máquinas, Veículos, Equipamentos e Ferramentas": return "B - Machinery, Vehicles, Equipment and Tools";
                case "Boas Condições de Uso": return "Good Conditions of Use";
                case "C - Programa Bom Senso": return "C - Good Sense Program";
                case "COGNITIVOS": return "COGNITIVES";
                case "Concentração na Tarefa": return "Task Concentration";
                case "Conhecimento Riscos Identificados pelo Programa Linha Mestra": return "Knowledge Risks Identified by the Master Line Program";
                case "Conhecimento e Consulta dos Procedimentos": return "Knowledge and Consultation of Procedures";
                case "Conservação Adequada": return "Proper Conservation";
                case "D - Acidentes, Incidentes e Desvios": return "D - Accidents, Incidents and Deviations";
                case "Descarte de Resíduos": return "Waste Disposal";
                case "Descrição da Atividade / Situação": return "Description of Activity / Situation";
                case "Destinados a Atividade (sem improvisação)": return "Intended for Activity (without improvisation)";
                case "Direito de Recusa": return "Right of Refusal";
                case "E - Planejamento, Procedimento e Instrução": return "E - Planning, Procedure and Instruction";
                case "Empresa Observada": return "Observed Company";
                case "FISIOLÓGICOS": return "PHYSIOLOGICAL";
                case "Gerência Abordagem": return "Management Approach";
                case "Identificação dos Riscos Mapeados": return "Identification of Mapped Risks";
                case "Identificação e tratativa dos riscos (Ex.: DNA e/ou Ver e Agir)": return "Identification and management of risks (Ex .: DNA and / or See and Act)";
                case "Inerentes a Atividade": return "Inherent in Activity";
                case "Linha de Fogo (Ex.: Distância, carga ou queda de algo)": return "Line of Fire (Ex .: Distance, load or fall of something)";
                case "Local Abordagem": return "Location Approach";
                case "Local Limpo": return "Clean Location";
                case "Materiais Organizados": return "Organized Materials";
                case "Medidas de Prevenção (Ex.: Inspeção e/ou ajustes)": return "Prevention Measures (Ex .: Inspection and / or adjustments)";
                case "Posturas Ergonômicas Adequadas": return "Proper Ergonomic Postures";
                case "Realiza os Processos de Rotina (Ex.: DDS e/ou Liberações)": return "Performs Routine Processes (Ex .: DDS and / or Releases)";
                case "Relação com os Riscos": return "Relationship with Risks";
                case "S.T. - Abordagem Comportamental": return "S.T. - Behavioral Approach";
                case "Unidade Abordagem": return "Unit Approach";
                case "Utilização Correta (Ex.: Fixação e/ou distância)": return "Correct Use (Ex .: Fixation and / or distance)";
                case "Utilização Correta": return "Correct Use";
                case "Ver e Agir": return "See and Act";
                case "Área Abordagem": return "Approach Area";
                case "A interação do gestor com sua equipe deve priorizar a abordagem do tema disponibilizado pela equipe de SSQV, porém é possível utilizar outro tema que seja pertinente para o objetivo do Programa. A avaliação dos itens abaixo sobre o tema abordado será de grande importância para que a relação com a equipe naquele momento seja completa.": return "A interação do gestor com sua equipe deve priorizar a abordagem do tema disponibilizado pela equipe de SSQV, porém é possível utilizar outro tema que seja pertinente para o objetivo do Programa. A avaliação dos itens abaixo sobre o tema abordado será de grande importância para que a relação com a equipe naquele momento seja completa.";
                case "Abordagem Comportamental": return "Behavioral Approach";
                case "Acompanhe seus ORTs": return "Track your ORTs";
                case "Acompanhe seus SNAs": return "Track your SNAs";
                case "Analise qual o nível de compreensão do tema apresentado. Utilize perguntas abertas neste momento. Qual nível de entendimento da equipe em termos da aplicação prática do conteúdo?": return "Analise qual o nível de compreensão do tema apresentado. Utilize perguntas abertas neste momento. Qual nível de entendimento da equipe em termos da aplicação prática do conteúdo?";
                case "Avaliação Descritiva": return "Descriptive Evaluation";
                case "Bom": return "Good";
                case "Cancelar": return "Cancel";
                case "Clima da Equipe": return "Team Climate";
                case "Conteúdo Abordado": return "Content Covered";
                case "Dados": return "Datas";
                case "Escolha uma ou duas ferramentas de segurança e pergunte para a equipe em relação a aplicação prática: se tem feito sentido, se o tempo dedicado é visto como oportunidade de evitar ocorrências e promover melhorias de ambientes e comportamentos, e quais as dúvidas ou sugestões. Como a equipe classifica a efetividade das ferramentas de segurança? Gentileza listar as ferramentas perguntadas.": return "Escolha uma ou duas ferramentas de segurança e pergunte para a equipe em relação a aplicação prática: se tem feito sentido, se o tempo dedicado é visto como oportunidade de evitar ocorrências e promover melhorias de ambientes e comportamentos, e quais as dúvidas ou sugestões. Como a equipe classifica a efetividade das ferramentas de segurança? Gentileza listar as ferramentas perguntadas.";
                case "Horário Final": return "End Time";
                case "Horário Inicial": return "Start Time";
                case "INTERAÇÃO DA GESTÃO NA FRENTE DE TRABALHO": return "INTERACTION OF MANAGEMENT ON THE WORK FRONT";
                case "Inicie o Segurança na Área se apresentando caso alguém não o conheça, pergunte como as pessoas estão, se tem alguma nova pessoa na equipe, aniversariante no dia ou semana. Como estava o clima da equipe?": return "Inicie o Segurança na Área se apresentando caso alguém não o conheça, pergunte como as pessoas estão, se tem alguma nova pessoa na equipe, aniversariante no dia ou semana. Como estava o clima da equipe?";
                case "Numero do DNA": return "DNA number";
                case "ORT": return "ORT";
                case "Observações Gerais": return "General observations";
                case "Qualidade dos ambientes de trabalho": return "Quality of work environments";
                case "Regular": return "Regular";
                case "Responsável": return "Responsible";
                case "Rotina da aplicação das ferramentas de segurança": return "Routine application of security tools";
                case "Ruim": return "Bad";
                case "SNA": return "SNA";
                case "Salvar": return "Save";
                case "Segurança na Área": return "Security in the Area";
                case "Tema Abordado": return "Topic Covered";
                case "Ver Abordagens Comportamentais cadastradas": return "See registered Behavioral Approaches";
                case "Ver SNAs cadastradas": return "View registered SNAs";
                case "Você considera o ambiente limpo, organizado, com condições de favorecimento de um clima positivo, que reforce a valorização das pessoas? Em termos gerais nestes aspectos, como você considera a área que visitou?": return "Você considera o ambiente limpo, organizado, com condições de favorecimento de um clima positivo, que reforce a valorização das pessoas? Em termos gerais nestes aspectos, como você considera a área que visitou?";

                default:
                    Log(texto);
                    return texto;
            }
        }

        private static string TraduzEs(string texto)
        {
            switch (texto)
            {
                case "Acompanhe suas Ocorrências": return "Rastrea tus eventos";
                case "Ajustes das configurações": return "Ajustes de configuración";
                case "Ajustes serão aplicados na próxima execução!": return "Los ajustes se aplicarán en la próxima ejecución!";
                case "Atualizando tabelas, progresso: ": return "Actualización de tablas, progreso: ";
                case "Aviso": return "Advertencia";
                case "Ações Tomadas": return "Acciones tomadas";
                case "Bem-vindo": return "Bienvenido";
                case "Campos obrigatórios": return "Campos obligatórios";
                case "Cancelar Registro": return "Cancelar Registro";
                case "Carregando...": return "Cargando...";
                case "Categoria da Ocorrência": return "Categoría de ocurrencia";
                case "Claro": return "Claro";
                case "Classificação da Ocorrência": return "Clasificación de ocurrencia";
                case "Comunicado por": return "Comunicado por";
                case "Configurações": return "Configuraciones";
                case "S.T. Suzano - Configurações": return "DNA - Ajustes";                
                case "DNA - De Olho na Área": return "DNA - vigilar el área";
                case "S.T. Suzano - Informações": return "Seguridad Suzano - Información";
                case "S.T. Suzano - Ocorrência": return "Seguridad Suzano - Suceso";
                case "S.T. Suzano - Ocorrências": return "Seguridad Suzano - Ocurrencias";
                case "Dados da Ocorrência": return "Datos de ocurrencia";
                case "Dados não salvos serão perdidos. Deseja continuar?": return "Los datos no guardados se perderán. Quieres continuar?";
                case "Data Inicial > Data Final.": return "Fecha de Inicio > Fecha de Finalización.";
                case "Data": return "Fecha";
                case "De Olho na Área": return "Vigilando el área";
                case "Descrever a ação imediata": return "Describir la acción inmediata.";
                case "Descrição da Ocorrência": return "Descripción del suceso";
                case "Digite ao menos 3 caracteres para efetuar o filtro!": return "¡Ingrese al menos 3 caracteres para realizar el filtro!";
                case "Digite seu usuário e senha": return "Ingrese su nombre de usuario y contraseña";
                case "Efetuar log out": return "Cerrar sesión";
                case "Entrar": return "Entrar";
                case "Erro ao salvar!": return "¡Error al guardar!";
                case "Erro": return "Error";
                case "Escuro": return "Oscuro";
                case "Estilo": return "Estilo";
                case "Foram Tomadas Ações Imediatas?": return "Se tomaron acciones inmediatas?";
                case "Fornecedor": return "Vendedor";
                case "Gerência do desvio": return "Gestión de derivación";
                case "Grande": return "Grande";
                case "Hora": return "Tiempo";
                case "Identificação": return "Identificación";
                case "Idioma": return "Idioma";
                case "Informações sobre o aplicativo": return "Información de la aplicación";
                case "Kbytes baixados: ": return "Kbytes descargados: ";
                case "Lembrar me": return "Recuerdame";
                case "Lembrar-me": return "Recuerdame";
                case "Limpar Dados": return "Borrar datos";
                case "Local do desvio": return "Desvío de ubicación";
                case "Localização": return "Ubicación";
                case "Médio": return "Medio";
                case "Nenhum registro encontrado com o filtro informado!": return "¡No se encontraron registros con el filtro dado!";
                case "Não existem registros para o período selecionado.": return "No hay registros para el período seleccionado.";
                case "Não quero me identificar": return "No quiero identificarme";
                case "Não": return "No";
                case "Número": return "Numero";
                case "Ok": return "Ok";
                case "Pequeno": return "Pequeño";
                case "Registrado Por": return "Registrado por";
                case "Registrar Ocorrência": return "Ocurrencia de registro";
                case "Registrar ocorrências": return "Registro de ocurrencias";
                case "Responsável pela Ocorrência": return "Responsable de la ocurrencia";
                case "Sair": return "Salir";
                case "Selecione a gerência do desvio!": return "Seleccione bypass management!";
                case "Selecione a unidade do desvio!": return "Seleccione la unidad de la desviación!";
                case "Selecione a área do desvio!": return "Seleccione el área de derivación!";
                case "Senha": return "Contraseña";
                case "Sim": return "Si";
                case "Sincronizar": return "Sincronizar";
                case "Sincronização": return "Sincronización";
                case "Sobre": return "Acerca de";
                case "Subclassificação da Ocorrência": return "Subclasificación de ocurrencia";
                case "Tamanho da fonte": return "Tamaño de fuente";
                case "Tipo de Ocorrência": return "Tipo de ocurrencia";
                case "Total de registros baixados: ": return "Total de registros descargados: ";
                case "Unidade do desvio": return "Unidad de desviacion";
                case "Usuário ou senha inválidos": return "Nombre de usuario o contraseña inválidos";
                case "Usuário": return "Usuario";
                case "Ver ocorrências cadastradas": return "Ver ocurrencias registradas";
                case "Voltar para a tela inicial": return "Regresar a la pantalla de inicio";
                case "Área do desvio": return "Área de desviación";
                case "Última atualização das tabelas: ": return "Tablas actualizadas por última vez: ";
                case "Refine a sua pesquisa para evitar travamentos.": return "Refina tu búsqueda para evitar accidentes.";
                case "Deseja continuar assim mesmo?": return "¿Quieres continuar de todos modos?";
                case "Mais de 1000 registros encontrados.": return "Más de 1000 registros encontrados.";
                case "Ocorrência cadastrada com sucesso!": return "Ocurrencias guardado con éxito!";
                case "Data Final": return "Fecha final";
                case "Data Inicial": return "La fecha de inicio";
                case "Não foi possível acessar a lista de ocorrências.": return "No se pudo acceder a la lista de resultados.";
                case "Ocorrências Cadastradas": return "Sucesos registrados";
                case "Ocorrências a serem enviadas": return "Ocurrencias a ser enviadas";
                case "Selecione o período": return "Selecciona el periodo";
                case "Verifique se o dispositivo possui conexão com a internet.": return "Verifique que el dispositivo tenga conexión a internet.";
                case "Cadastradas": return "Registrados";
                case "Pendentes": return "Pendiente";
                case "Ocorrência '{0}' cadastrada com sucesso!": return "¡Ocurrencia '{0}' guardado con éxito!";
                case "Erro ao sincronizar.": return "Error al sincronizar";
                case "Mais de 1000 registros encontrados. Refine a sua pesquisa.": return "Más de 1000 registros encontrados. Refina tu búsqueda.";
                case "Não foi possível obter os dados da ocorrência.": return "No fue posible obtener los datos de ocurrencia.";
                case "Não foi possível sincronizar.": return "No se pudo sincronizar.";
                case "Selecione classificação da ocorrência!": return "Seleccione clasificación de ocurrencia!";
                case "Selecione subclassificação da ocorrência!": return "Seleccione subclasificación de la ocurrencia!";
                case "Tente novamente mais tarde.": return "Intenta nuevamente más tarde.";
                case "A - Equipamento de Proteção Individual e Coletivo": return "A - Equipos de protección individual y colectiva";
                case "Ativadores de Comportamento": return "Activadores de comportamiento";
                case "Ações em Emergência (Ex.: Rota de Fuga, Chuveiro, Telefone, Brigadista, Alergia)": return "Acciones de emergencia (por ejemplo, ruta de escape, ducha, teléfono, brigada, alergia)";
                case "B - Máquinas, Veículos, Equipamentos e Ferramentas": return "B - Maquinaria, vehículos, equipos y herramientas";
                case "Boas Condições de Uso": return "Buenas condiciones de uso";
                case "C - Programa Bom Senso": return "C - Programa de buen sentido";
                case "COGNITIVOS": return "COGNITIVAS";
                case "Concentração na Tarefa": return "Concentración de tareas";
                case "Conhecimento Riscos Identificados pelo Programa Linha Mestra": return "Riesgos de conocimiento identificados por el programa Master Line";
                case "Conhecimento e Consulta dos Procedimentos": return "Conocimiento y consulta de procedimientos";
                case "Conservação Adequada": return "Conservación adecuada";
                case "D - Acidentes, Incidentes e Desvios": return "D - Accidentes, incidentes y desviaciones";
                case "Descarte de Resíduos": return "Deposito de basura";
                case "Descrição da Atividade / Situação": return "Descripción de la actividad / situación";
                case "Destinados a Atividade (sem improvisação)": return "Destinado a la actividad (sin improvisación)";
                case "Direito de Recusa": return "Derecho de rechazo";
                case "E - Planejamento, Procedimento e Instrução": return "E - Planificación, procedimiento e instrucción";
                case "Empresa Observada": return "Compañía Observada";
                case "FISIOLÓGICOS": return "FISIOLÓGICO";
                case "Gerência Abordagem": return "Enfoque de gestión";
                case "Identificação dos Riscos Mapeados": return "Identificación de riesgos mapeados";
                case "Identificação e tratativa dos riscos (Ex.: DNA e/ou Ver e Agir)": return "Identificación y manejo de riesgos (Ej .: ADN y / o Ver y Actuar)";
                case "Inerentes a Atividade": return "Inherente a la actividad";
                case "Linha de Fogo (Ex.: Distância, carga ou queda de algo)": return "Línea de fuego (Ej .: Distancia, carga o caída de algo)";
                case "Local Abordagem": return "Enfoque de ubicación";
                case "Local Limpo": return "Ubicación limpia";
                case "Materiais Organizados": return "Materiales organizados";
                case "Medidas de Prevenção (Ex.: Inspeção e/ou ajustes)": return "Medidas de prevención (Ej .: Inspección y / o ajustes)";
                case "Posturas Ergonômicas Adequadas": return "Posturas ergonómicas adecuadas";
                case "Realiza os Processos de Rotina (Ex.: DDS e/ou Liberações)": return "Realiza procesos de rutina (ej .: DDS y / o versiones)";
                case "Relação com os Riscos": return "Relación con los riesgos";
                case "S.T. - Abordagem Comportamental": return "S.T. - Enfoque conductual";
                case "Unidade Abordagem": return "Enfoque de la unidad";
                case "Utilização Correta (Ex.: Fixação e/ou distância)": return "Uso correcto (Ej .: Fijación y / o distancia)";
                case "Utilização Correta": return "Uso correcto";
                case "Ver e Agir": return "Ver y actuar";
                case "Área Abordagem": return "Área de aproximación";
                case "A interação do gestor com sua equipe deve priorizar a abordagem do tema disponibilizado pela equipe de SSQV, porém é possível utilizar outro tema que seja pertinente para o objetivo do Programa. A avaliação dos itens abaixo sobre o tema abordado será de grande importância para que a relação com a equipe naquele momento seja completa.": return "A interação do gestor com sua equipe deve priorizar a abordagem do tema disponibilizado pela equipe de SSQV, porém é possível utilizar outro tema que seja pertinente para o objetivo do Programa. A avaliação dos itens abaixo sobre o tema abordado será de grande importância para que a relação com a equipe naquele momento seja completa.";
                case "Abordagem Comportamental": return "Enfoque conductual";
                case "Acompanhe seus ORTs": return "Seguimiento de sus ORT";
                case "Acompanhe seus SNAs": return "Seguimiento de sus SLZ";
                case "Analise qual o nível de compreensão do tema apresentado. Utilize perguntas abertas neste momento. Qual nível de entendimento da equipe em termos da aplicação prática do conteúdo?": return "Analise qual o nível de compreensão do tema apresentado. Utilize perguntas abertas neste momento. Qual nível de entendimento da equipe em termos da aplicação prática do conteúdo?";
                case "Avaliação Descritiva": return "Evaluación descriptiva";
                case "Bom": return "Bueno";
                case "Cancelar": return "Cancelar";
                case "Clima da Equipe": return "Clima del equipo";
                case "Conteúdo Abordado": return "Contenido cubierto";
                case "Dados": return "Dado";
                case "Escolha uma ou duas ferramentas de segurança e pergunte para a equipe em relação a aplicação prática: se tem feito sentido, se o tempo dedicado é visto como oportunidade de evitar ocorrências e promover melhorias de ambientes e comportamentos, e quais as dúvidas ou sugestões. Como a equipe classifica a efetividade das ferramentas de segurança? Gentileza listar as ferramentas perguntadas.": return "Escolha uma ou duas ferramentas de segurança e pergunte para a equipe em relação a aplicação prática: se tem feito sentido, se o tempo dedicado é visto como oportunidade de evitar ocorrências e promover melhorias de ambientes e comportamentos, e quais as dúvidas ou sugestões. Como a equipe classifica a efetividade das ferramentas de segurança? Gentileza listar as ferramentas perguntadas.";
                case "Horário Final": return "Hora de finalización";
                case "Horário Inicial": return "Hora de inicio";
                case "INTERAÇÃO DA GESTÃO NA FRENTE DE TRABALHO": return "INTERACCIÓN DE LA GESTIÓN EN EL FRENTE DE TRABAJO";
                case "Inicie o Segurança na Área se apresentando caso alguém não o conheça, pergunte como as pessoas estão, se tem alguma nova pessoa na equipe, aniversariante no dia ou semana. Como estava o clima da equipe?": return "Inicie o Segurança na Área se apresentando caso alguém não o conheça, pergunte como as pessoas estão, se tem alguma nova pessoa na equipe, aniversariante no dia ou semana. Como estava o clima da equipe?";
                case "Numero do DNA": return "Número de ADN";
                case "ORT": return "ORT";
                case "Observações Gerais": return "Observaciones generales";
                case "Qualidade dos ambientes de trabalho": return "Calidad de los entornos laborales";
                case "Regular": return "Regular";
                case "Responsável": return "Responsable";
                case "Rotina da aplicação das ferramentas de segurança": return "Aplicación rutinaria de herramientas de seguridad";
                case "Ruim": return "Malo";
                case "SNA": return "SLZ";
                case "Salvar": return "Ahorrar";
                case "Segurança na Área": return "Seguridad en la Zona";
                case "Tema Abordado": return "Tema cubierto";
                case "Ver Abordagens Comportamentais cadastradas": return "Ver enfoques conductuales registrados";
                case "Ver SNAs cadastradas": return "Ver SLZ registrados";
                case "Você considera o ambiente limpo, organizado, com condições de favorecimento de um clima positivo, que reforce a valorização das pessoas? Em termos gerais nestes aspectos, como você considera a área que visitou?": return "Você considera o ambiente limpo, organizado, com condições de favorecimento de um clima positivo, que reforce a valorização das pessoas? Em termos gerais nestes aspectos, como você considera a área que visitou?";

                default:
                    Log(texto);
                    return texto;
            }
        }

        private static string TraduzFr(string texto)
        {
            switch (texto)
            {
                case "Acompanhe suas Ocorrências": return "Suivez vos événements";
                case "Ajustes das configurações": return "Réglages des paramètres";
                case "Ajustes serão aplicados na próxima execução!": return "Des ajustements seront appliqués lors de la prochaine exécution!";
                case "Atualizando tabelas, progresso: ": return "Mise à jour des tableaux, progression: ";
                case "Aviso": return "Avertissement";
                case "Ações Tomadas": return "Les mesures prises";
                case "Bem-vindo": return "Bienvenue";
                case "Campos obrigatórios": return "Champs obligatoires";
                case "Cancelar Registro": return "Annuler l'inscription";
                case "Carregando...": return "Chargement...";
                case "Categoria da Ocorrência": return "Catégorie d'occurrence";
                case "Claro": return "Bien sûr";
                case "Classificação da Ocorrência": return "Classification de l'événement";
                case "Comunicado por": return "Communiqué par";
                case "Configurações": return "Paramètres";
                case "S.T. Suzano - Configurações": return "S.T. Suzano - Réglages";
                case "DNA - De Olho Na Área": return "DNA - Garder un œil sur la zone";                
                case "S.T. Suzano - Informações": return "S.T. Suzano - Informations";
                case "S.T. Suzano - Ocorrência": return "S.T. Suzano - Occurrence";
                case "S.T. Suzano - Ocorrências": return "S.T. Suzano - Occurrences";
                case "Dados da Ocorrência": return "Données d'occurrence";
                case "Dados não salvos serão perdidos. Deseja continuar?": return "Les données non enregistrées seront perdues. Voulez-vous continuer?";
                case "Data Inicial > Data Final.": return "Date de Début > Date de Fin.";
                case "Data": return "Date";
                case "De Olho na Área": return "Garder un œil sur la zone";
                case "Descrever a ação imediata": return "Décrire l'action immédiate";
                case "Descrição da Ocorrência": return "Description de l'événement";
                case "Digite ao menos 3 caracteres para efetuar o filtro!": return "Entrez au moins 3 caractères pour effectuer le filtre!";
                case "Digite seu usuário e senha": return "Entrez votre nom d'utilisateur et votre mot de passe";
                case "Efetuar log out": return "Se déconnecter";
                case "Entrar": return "Entre";
                case "Erro ao salvar!": return "Erreur d'enregistrement!";
                case "Erro": return "Erreur";
                case "Escuro": return "Sombre";
                case "Estilo": return "Le style";
                case "Foram Tomadas Ações Imediatas?": return "Des mesures immédiates ont-elles été prises?";
                case "Fornecedor": return "Vendeur";
                case "Gerência do desvio": return "Gestion du bypass";
                case "Grande": return "Grand";
                case "Hora": return "Le temps";
                case "Identificação": return "Identification";
                case "Idioma": return "La langue";
                case "Informações sobre o aplicativo": return "Informations sur l'application";
                case "Kbytes baixados: ": return "Koctets téléchargés";
                case "Lembrar me": return "Souviens toi de moi";
                case "Lembrar-me": return "Souviens toi de moi";
                case "Limpar Dados": return "Effacer les données";
                case "Local do desvio": return "Lieu du détour";
                case "Localização": return "Emplacement";
                case "Médio": return "Moyen";
                case "Nenhum registro encontrado com o filtro informado!": return "Aucun enregistrement trouvé avec le filtre donné!";
                case "Não existem registros para o período selecionado.": return "Il n'y a aucun enregistrement pour la période sélectionnée.";
                case "Não quero me identificar": return "Je ne veux pas m'identifier";
                case "Não": return "Non";
                case "Número": return "Numéro";
                case "Ok": return "Ok";
                case "Pequeno": return "Petit";
                case "Registrado Por": return "Enregistré par";
                case "Registrar Ocorrência": return "Enregistrer l'occurrence";
                case "Registrar ocorrências": return "Occurrences de journal";
                case "Responsável pela Ocorrência": return "Responsable de l'occurrence";
                case "Sair": return "Quitter";
                case "Selecione a gerência do desvio!": return "Sélectionnez la gestion du contournement!";
                case "Selecione a unidade do desvio!": return "Sélectionnez l'unité de l'écart!";
                case "Selecione a área do desvio!": return "Sélectionnez la zone de contournement!";
                case "Senha": return "Passe";
                case "Sim": return "Ouais";
                case "Sincronizar": return "Synchroniser";
                case "Sincronização": return "Synchronisation";
                case "Sobre": return "À propos";
                case "Subclassificação da Ocorrência": return "Sous-classification de l'occurrence";
                case "Tamanho da fonte": return "Taille de police";
                case "Tipo de Ocorrência": return "Type d'occurrence";
                case "Total de registros baixados: ": return "Total d'enregistrements téléchargés: ";
                case "Unidade do desvio": return "Unité de déviation";
                case "Usuário ou senha inválidos": return "Nom d'utilisateur ou mot de passe invalide";
                case "Usuário": return "D'utilisateur";
                case "Ver ocorrências cadastradas": return "Afficher les événements enregistrés";
                case "Voltar para a tela inicial": return "Revenir à l'écran d'accueil";
                case "Área do desvio": return "Zone de déviation";
                case "Última atualização das tabelas: ": return "Dernière mise à jour des tableaux: ";
                case "Mais de 1000 registros encontrados.": return "Plus de 1000 enregistrements trouvés.";
                case "Refine a sua pesquisa para evitar travamentos.": return "Affinez votre recherche pour éviter les plantages.";
                case "Deseja continuar assim mesmo?": return "Voulez-vous quand même continuer?";
                case "Ocorrência cadastrada com sucesso!": return "occurrence enregistrée avec succès!";
                case "Data Final": return "Date finale";
                case "Data Inicial": return "Date initiale";
                case "Não foi possível acessar a lista de ocorrências.": return "Impossible d'accéder à la liste des résultats.";
                case "Ocorrências Cadastradas": return "Occurrences enregistrées";
                case "Ocorrências a serem enviadas": return "Occurrences à envoyer";
                case "Selecione o período": return "Sélectionnez la période";
                case "Verifique se o dispositivo possui conexão com a internet.": return "Vérifiez que l'appareil dispose d'une connexion Internet.";
                case "Cadastradas": return "Enregistrées";
                case "Pendentes": return "En attente";
                case "Ocorrência '{0}' cadastrada com sucesso!": return "L'occurrence '{0}' a bien été enregistrée!";
                case "Erro ao sincronizar.": return "Erreur lors de la synchronisation.";
                case "Mais de 1000 registros encontrados. Refine a sua pesquisa.": return "Plus de 1000 enregistrements trouvés. Précisez votre recherche.";
                case "Não foi possível obter os dados da ocorrência.": return "Il n'a pas été possible d'obtenir les données d'occurrence.";
                case "Não foi possível sincronizar.": return "Impossible de synchroniser.";
                case "Selecione classificação da ocorrência!": return "Sélectionnez la classification d'occurrence!";
                case "Selecione subclassificação da ocorrência!": return "Sélectionnez la sous-classification de l'occurrence!";
                case "Tente novamente mais tarde.": return "Réessayez plus tard.";
                case "A - Equipamento de Proteção Individual e Coletivo": return "A - Équipements de protection individuelle et collective";
                case "Ativadores de Comportamento": return "Activateurs de comportement";
                case "Ações em Emergência (Ex.: Rota de Fuga, Chuveiro, Telefone, Brigadista, Alergia)": return "Actions d'urgence (par ex. Itinéraire d'évacuation, douche, téléphone, brigade, allergie)";
                case "B - Máquinas, Veículos, Equipamentos e Ferramentas": return "B - Machines, véhicules, équipements et outils";
                case "Boas Condições de Uso": return "Bonnes conditions d'utilisation";
                case "C - Programa Bom Senso": return "C - Programme Good Sense";
                case "COGNITIVOS": return "COGNITIFS";
                case "Concentração na Tarefa": return "Concentration des tâches";
                case "Conhecimento Riscos Identificados pelo Programa Linha Mestra": return "Risques liés aux connaissances identifiés par le programme Master Line";
                case "Conhecimento e Consulta dos Procedimentos": return "Connaissance et consultation des procédures";
                case "Conservação Adequada": return "Une bonne conservation";
                case "D - Acidentes, Incidentes e Desvios": return "D - Accidents, incidents et écarts";
                case "Descarte de Resíduos": return "Traitement des déchets";
                case "Descrição da Atividade / Situação": return "Description de l'activité / de la situation";
                case "Destinados a Atividade (sem improvisação)": return "Destiné à l'activité (sans improvisation)";
                case "Direito de Recusa": return "Droit de refus";
                case "E - Planejamento, Procedimento e Instrução": return "E - Planification, procédure et instruction";
                case "Empresa Observada": return "Société observée";
                case "FISIOLÓGICOS": return "PHYSIOLOGIQUE";
                case "Gerência Abordagem": return "Approche de gestion";
                case "Identificação dos Riscos Mapeados": return "Identification des risques cartographiés";
                case "Identificação e tratativa dos riscos (Ex.: DNA e/ou Ver e Agir)": return "Identification et gestion des risques (Ex.: DNA et / ou See and Act)";
                case "Inerentes a Atividade": return "Inhérent à l'activité";
                case "Linha de Fogo (Ex.: Distância, carga ou queda de algo)": return "Ligne de tir (Ex.: Distance, charge ou chute de quelque chose)";
                case "Local Abordagem": return "Approche de l'emplacement";
                case "Local Limpo": return "Emplacement propre";
                case "Materiais Organizados": return "Matériaux organisés";
                case "Medidas de Prevenção (Ex.: Inspeção e/ou ajustes)": return "Mesures de prévention (Ex.: Inspection et / ou ajustements)";
                case "Posturas Ergonômicas Adequadas": return "Postures ergonomiques appropriées";
                case "Realiza os Processos de Rotina (Ex.: DDS e/ou Liberações)": return "Effectue des processus de routine (ex.: DDS et / ou versions)";
                case "Relação com os Riscos": return "Relation avec les risques";
                case "S.T. - Abordagem Comportamental": return "S.T. - Approche comportementale";
                case "Unidade Abordagem": return "Approche de l'unité";
                case "Utilização Correta (Ex.: Fixação e/ou distância)": return "Utilisation correcte (Ex.: Fixation et / ou distance)";
                case "Utilização Correta": return "Utilisation correcte";
                case "Ver e Agir": return "Voir et agir";
                case "Área Abordagem": return "Zone d'approche";
                case "A interação do gestor com sua equipe deve priorizar a abordagem do tema disponibilizado pela equipe de SSQV, porém é possível utilizar outro tema que seja pertinente para o objetivo do Programa. A avaliação dos itens abaixo sobre o tema abordado será de grande importância para que a relação com a equipe naquele momento seja completa.": return "A interação do gestor com sua equipe deve priorizar a abordagem do tema disponibilizado pela equipe de SSQV, porém é possível utilizar outro tema que seja pertinente para o objetivo do Programa. A avaliação dos itens abaixo sobre o tema abordado será de grande importância para que a relação com a equipe naquele momento seja completa.";
                case "Abordagem Comportamental": return "Approche comportementale";
                case "Acompanhe seus ORTs": return "Suivez vos ORT";
                case "Acompanhe seus SNAs": return "Suivez vos SLZ";
                case "Analise qual o nível de compreensão do tema apresentado. Utilize perguntas abertas neste momento. Qual nível de entendimento da equipe em termos da aplicação prática do conteúdo?": return "Analise qual o nível de compreensão do tema apresentado. Utilize perguntas abertas neste momento. Qual nível de entendimento da equipe em termos da aplicação prática do conteúdo?";
                case "Avaliação Descritiva": return "Évaluation descriptive";
                case "Bom": return "Bien";
                case "Cancelar": return "Annuler";
                case "Clima da Equipe": return "Climat d'équipe";
                case "Conteúdo Abordado": return "Contenu couvert";
                case "Dados": return "Dé";
                case "Escolha uma ou duas ferramentas de segurança e pergunte para a equipe em relação a aplicação prática: se tem feito sentido, se o tempo dedicado é visto como oportunidade de evitar ocorrências e promover melhorias de ambientes e comportamentos, e quais as dúvidas ou sugestões. Como a equipe classifica a efetividade das ferramentas de segurança? Gentileza listar as ferramentas perguntadas.": return "Escolha uma ou duas ferramentas de segurança e pergunte para a equipe em relação a aplicação prática: se tem feito sentido, se o tempo dedicado é visto como oportunidade de evitar ocorrências e promover melhorias de ambientes e comportamentos, e quais as dúvidas ou sugestões. Como a equipe classifica a efetividade das ferramentas de segurança? Gentileza listar as ferramentas perguntadas.";
                case "Horário Final": return "Heure de fin";
                case "Horário Inicial": return "Heure de début";
                case "INTERAÇÃO DA GESTÃO NA FRENTE DE TRABALHO": return "INTERACTION DE LA GESTION EN FRONT DE TRAVAIL";
                case "Inicie o Segurança na Área se apresentando caso alguém não o conheça, pergunte como as pessoas estão, se tem alguma nova pessoa na equipe, aniversariante no dia ou semana. Como estava o clima da equipe?": return "Inicie o Segurança na Área se apresentando caso alguém não o conheça, pergunte como as pessoas estão, se tem alguma nova pessoa na equipe, aniversariante no dia ou semana. Como estava o clima da equipe?";
                case "Numero do DNA": return "Numéro ADN";
                case "ORT": return "ORT";
                case "Observações Gerais": return "Observations générales";
                case "Qualidade dos ambientes de trabalho": return "Qualité des environnements de travail";
                case "Regular": return "Ordinaire";
                case "Responsável": return "Responsable";
                case "Rotina da aplicação das ferramentas de segurança": return "Application courante des outils de sécurité";
                case "Ruim": return "Mal";
                case "SNA": return "SLZ";
                case "Salvar": return "Sauver";
                case "Segurança na Área": return "Sécurité dans la zone";
                case "Tema Abordado": return "Sujet couvert";
                case "Ver Abordagens Comportamentais cadastradas": return "Voir les approches comportementales enregistrées";
                case "Ver SNAs cadastradas": return "Afficher les SNA enregistrés";
                case "Você considera o ambiente limpo, organizado, com condições de favorecimento de um clima positivo, que reforce a valorização das pessoas? Em termos gerais nestes aspectos, como você considera a área que visitou?": return "Você considera o ambiente limpo, organizado, com condições de favorecimento de um clima positivo, que reforce a valorização das pessoas? Em termos gerais nestes aspectos, como você considera a área que visitou?";

                default:
                    Log(texto);
                    return texto;
            }
        }

        private static void Log(string texto)
        {
            return;

            if (Device.RuntimePlatform == Device.UWP && Debugger.IsAttached && !_log.Contains(texto))
            {
                _log.Add(texto);

                string traducao = TraduzApi(texto, "pt", "en");
                Debug.WriteLine($"Globalização en:                case \"{texto}\": return \"{traducao}\";");

                traducao = TraduzApi(texto, "pt", "es");
                Debug.WriteLine($"Globalização es:                case \"{texto}\": return \"{traducao}\";");

                traducao = TraduzApi(texto, "pt", "fr");
                Debug.WriteLine($"Globalização fr:                case \"{texto}\": return \"{traducao}\";");
            }
        }

        private static string TraduzApi(string texto, string idiomaOrigem, string idiomaDestino )
        {
            string url = string.Empty;
            try
            {
                string textoEncoded = HttpUtility.UrlEncode(texto);
                url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={idiomaOrigem}&tl={idiomaDestino}&dt=t&q={textoEncoded}";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";
                //request.UserAgent = RequestConstants.UserAgentValue;
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                string conteudo = string.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            conteudo = sr.ReadToEnd();
                            if (conteudo.Contains(texto))
                            {
                                int inicio = conteudo.IndexOf('"') + 1;
                                int final = conteudo.IndexOf("\",\"") - 4;

                                texto = conteudo.Substring(inicio, final);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(url);
            }

            return texto;
        }
    }
}
