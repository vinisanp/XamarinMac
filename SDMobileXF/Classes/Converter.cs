using Plugin.Media.Abstractions;
using SDMobileXF.Banco.Tabelas;
using SDMobileXFDados.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace SDMobileXF.Classes
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            switch (value)
            {
                case 0: return Color.Black; //Sem aceite
                case 1: return Color.FromHex("#02c308"); //Ação imediata
                case 2: return Color.Red; //Sem plano de ação
                case 3: return Color.DarkTurquoise; //Vencido
                case 4: return Color.Blue; //Em andamento
                case 5: return Color.Orange; //A executar
                case 6: return Color.Green; //Liberado para eficácia
                case 7: return Color.DarkViolet; //Realizado                
                case 8: return Color.DarkGray; //Rejeitada
                default: return Color.White;
            }            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DescricaoStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case 0: return "Sem aceite";
                case 1: return "Ação imediata";
                case 2: return "Sem plano de ação";
                case 3: return "Vencido";
                case 4: return "Em andamento";
                case 5: return "A executar";
                case 6: return "Liberado para eficácia";                
                case 7: return "Realizado";
                case 9: return "Rejeitada";
                default: return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DataToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                DateTime data = System.Convert.ToDateTime(value);
                if (data.TimeOfDay.TotalSeconds > 0)
                    return string.Concat(data.ToString(culture.DateTimeFormat.ShortDatePattern), " ", data.ToString(culture.DateTimeFormat.ShortTimePattern));
                return data.ToString(culture.DateTimeFormat.ShortDatePattern);
            }
            else
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
