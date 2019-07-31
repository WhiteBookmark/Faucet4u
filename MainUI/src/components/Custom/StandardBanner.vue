<template>
    <div v-html="standardBanner"></div>
</template>

<script>
    export default {
        name: "StandardBanner",
        data() {
            return {
                standardBanner: null
            };
        },
        async mounted() {

            try {
                const response = await this.axios.get(process.env.VUE_APP_API_BannerNetworkHTMLCode);
                this.standardBanner = response.data.HTMLCode;
            }
            catch (error) {
                console.log(error);
            }
        }
    };
</script>
