const colors = require('tailwindcss/colors')
module.exports = {
  purge: ['./src/**/*.html', './src/**/*.ts', './projects/**/*.html', './projects/**/*.ts'],
  theme: {
    extend: {
      colors: {
        'light-blue': colors.lightBlue,
        teal: colors.teal,
      }
    }
  },
  variants: {},
  plugins: [
    require('@tailwindcss/forms'),
  ],
}
