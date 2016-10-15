window.VueStrapLang = (function(){
var l = { //languages

en: {
  daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
  limit: 'Limit reached ({{limit}} items max).',
  loading: 'Loading...',
  minLength: 'Min. Length',
  months: [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'
  ],
  notSelected: 'Nothing Selected',
  required: 'Required',
  search: 'Search'
},
cn: {
    daysOfWeek: ['日','一', '二', '三', '四', '五', '六'],
    limit: 'Limit reached ({{limit}} items max).',
    loading: 'Loading...',
    minLength: '最小长度',
    months: [
      '1月', '2月', '3月', '4月', '5月', '6月',
      '7月', '8月', '9月', '10月', '11月', '12月'
    ],
    notSelected: '-请选择-',
    required: '必填',
    search: '查询'
},

es: {
  daysOfWeek: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
  loading: 'Cargando...',
  minLength: 'Tamaño Mínimo',
  months: [
    'Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
    'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'
  ],
  notSelected: 'Nada seleccionado',
  required: 'Requerido',
  search: 'Buscar',
  limit: 'Limite alcanzado (máximo {{limit}} items).',
},

'pt-BR': {
  daysOfWeek: ['Do', 'Se', 'Te', 'Qa', 'Qi', 'Sx', 'Sa'],
  limit: 'Limite atingido (máximo {{limit}} items).',
  loading: 'Cargando...',
  minLength: 'Tamanho Mínimo',
  months: [
    'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
    'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'
  ],
  notSelected: 'Nada selecionado',
  required: 'Requerido',
  search: 'Buscar',
},

fr: {
  daysOfWeek: ['Di', 'Lu', 'Ma', 'Me', 'Je', 'Ve', 'Sa'],
  limit: 'Limite atteinte ({{limit}} éléments max).',
  loading: 'Chargement en cours...',
  minLength: 'Longueur Minimum',
  months: [
    'Janvier', 'Février', 'Mars', 'Avril', 'Mai', 'Juin',
    'Juillet', 'Août', 'Septembre', 'Octobre', 'Novembre', 'Décembre'
  ],
  notSelected: 'Aucune sélection',
  required: 'Requis',
  search: 'Recherche'
}

};

/**
 * Some browsers handle short language code (eg. 'en'), others handle 5 chars (eg. 'en-US').
 * With aliases you can handle a group of similar languages, using a regular expresion.
 * If the language is not found, the default language is english.
 */
var aliases = {
  es: /^es-[A-Z]{2}$/i,
  en: /^en-[A-Z]{2}$/i,
  cn: /^cn-[A-Z]{2}$/i
};

return function (lang) {
    // lang = lang || 'en';
    lang = lang || 'cn';
  var i, tr = {};
  for (i in aliases) {
    if (aliases[i].test(lang)) lang = i;
  }
  for (i in l.en) {
    tr[i] = (l[lang] && l[lang][i]) || l.en[i];
  }

  return tr;
};
})();
