module.exports = {
  purge: ['./src/**/*.html', './src/**/*.ts', './projects/**/*.html', './projects/**/*.ts'],
  theme: {
    extend: {},
  },
  variants: {},
  plugins: [
    require('@tailwindcss/forms'),
  ],
}
