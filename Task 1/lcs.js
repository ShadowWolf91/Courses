a = process.argv.slice(2);s = a[0];if (!s) s = "";for (i = s.length; ~i; i--) {for (j = 0; j <= s.length - i; j++) {t = s.slice(j, j + i);f = true;for (x = 0; x < a.length; x++) if (!a[x].includes(t)) f = false;if (f) {console.log(t);return;}}}